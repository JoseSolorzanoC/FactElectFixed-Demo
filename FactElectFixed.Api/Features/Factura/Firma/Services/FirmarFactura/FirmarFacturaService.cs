using System.Xml;
using FactElectFixed.Api.Features.Factura.Firma.Mappers;
using FactElectFixed.Api.Features.Factura.Firma.Reponses;
using FactElectFixed.Api.Features.Factura.Firma.Requests;
using FactElectFixed.Api.Features.Factura.Firma.Xml;
using FactElectFixed.Api.Helpers;
using FactElectFixed.Api.Services;
using Infoware.SRI.Core.Helpers;
using Infoware.SRI.Firmar;
using Infoware.SRI.WebService;
using Infoware.SRI.WebService.Response;
using Microsoft.Extensions.Logging.Abstractions;

namespace FactElectFixed.Api.Features.Factura.Firma.Services.FirmarFactura;

public class FirmarFacturaService(
    ISRIWebService sriWebService,
    IWebHostEnvironment webHostEnvironment,
    ICachedXmlFileService cachedXmlFileService)
    : IFirmarFacturaService
{
    private XmlDocument SerializarXml(FirmarFacturaRequest firmarFacturaRequest, out string? rutaXmlExistente)
    {
        ArgumentNullException.ThrowIfNull(firmarFacturaRequest);

        FacturaXmlModel facturaXmlModel = firmarFacturaRequest.FacturaData.ToXml();

        rutaXmlExistente = BuscarXmlPorClaveDeAcceso(facturaXmlModel.InfoTributaria.ClaveAcceso);

        return facturaXmlModel.ToXmlDocument();
    }

    private XmlDocument FirmarXml(XmlDocument xmlFactura)
    {
        var certificadoService = new CertificadoService(new NullLogger<CertificadoService>());

        try
        {
            string? rutaCertificado = BuscarCertificadoPorRuc("1207245927001");
            if (rutaCertificado is null)
            {
                throw new Exception("No se pudo encontrar un certificado asociado al RUC de la empresa");
            }

            certificadoService.CargarDesdeP12(rutaCertificado,
                "8736Th3IxR");

            return certificadoService.FirmarDocumento(xmlFactura);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<EnviarFacturaSriResponse> EnviarFacturaSri(FirmarFacturaRequest firmarFacturaRequest)
    {
        try
        {
            XmlDocument xmlFactura = SerializarXml(firmarFacturaRequest, out string? rutaXmlExistente);

            if (rutaXmlExistente is not null)
            {
                return new EnviarFacturaSriResponse
                {
                    Success = true,
                    MensajesSriRecepcion = [],
                    ClaveAcceso = rutaXmlExistente.Split("_")[3],
                    XmlProcesado = cachedXmlFileService.GetXml(rutaXmlExistente)
                };
            }

            XmlDocument xmlFacturaFirmado = FirmarXml(xmlFactura);

            Response<ValidarComprobanteResponse.RespuestaRecepcionComprobante> respuesta =
                await sriWebService.ValidarComprobanteAsync(xmlFacturaFirmado);

            if (respuesta is { Ok: false, Data.Comprobantes.Comprobante: not null })
            {
                var mensajesSri = new List<ValidarComprobanteResponse.Mensaje>();
                foreach (ValidarComprobanteResponse.Comprobante errorSri in respuesta.Data.Comprobantes.Comprobante)
                {
                    if (errorSri is { Mensajes.Mensaje: not null })
                    {
                        mensajesSri.AddRange(errorSri.Mensajes.Mensaje);
                    }
                }

                return new EnviarFacturaSriResponse
                {
                    Success = false,
                    MensajesSriRecepcion = mensajesSri,
                };
            }

            XmlNode? claveNode = xmlFacturaFirmado.SelectSingleNode("//infoTributaria/claveAcceso");

            string claveAcceso = string.Empty;

            if (claveNode is not null)
            {
                claveAcceso = claveNode.InnerText.Trim();

                VerificarFacturaResponse verificarFacturaResponse =
                    await VerificarFacturaSri(claveAcceso, xmlFacturaFirmado.OuterXml);

                if (verificarFacturaResponse.Success)
                {
                    SafeXmlFileWriter.SaveXml(ConstruirRutaXml(claveAcceso), xmlFacturaFirmado.OuterXml);

                    return new EnviarFacturaSriResponse
                    {
                        Success = true,
                        ClaveAcceso = claveAcceso,
                        MensajesSriAutorizacion = verificarFacturaResponse.MensajesSri,
                        MensajesSriRecepcion = [],
                        XmlProcesado = xmlFacturaFirmado.OuterXml
                    };
                }

                return new EnviarFacturaSriResponse
                {
                    Success = false,
                    ClaveAcceso = claveAcceso,
                    MensajesSriAutorizacion = verificarFacturaResponse.MensajesSri,
                    MensajesSriRecepcion = [],
                };
            }

            return new EnviarFacturaSriResponse
            {
                Success = false,
                ClaveAcceso = claveAcceso,
                MensajesSriAutorizacion = []
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<VerificarFacturaResponse> VerificarFacturaSri(string claveAcceso, string xmlFacturaFirmado)
    {
        try
        {
            Response<AutorizarComprobanteResponse.RespuestaAutorizacionComprobante> respuestaClave =
                await sriWebService.AutorizacionComprobanteAsync(claveAcceso);

            if (respuestaClave.Ok)
            {
                return new VerificarFacturaResponse
                {
                    Success = true,
                    ClaveAcceso = claveAcceso,
                    XmlProcesado = xmlFacturaFirmado,
                    Respuesta = respuestaClave.Data?.Autorizaciones?.Autorizacion?.FirstOrDefault()?.Estado ??
                                "INCONCLUSO"
                };
            }

            return new VerificarFacturaResponse
            {
                Success = false,
                ClaveAcceso = claveAcceso,
                XmlProcesado = xmlFacturaFirmado,
                Respuesta = respuestaClave.Data?.Autorizaciones?.Autorizacion?.FirstOrDefault()?.Estado ?? "INCONCLUSO",
                MensajesSri = respuestaClave.Data?.Autorizaciones?.Autorizacion?.Where(a => a.Mensajes?.Mensaje != null)
                    .SelectMany(a => a.Mensajes!.Mensaje!).ToList()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private string? BuscarCertificadoPorRuc(string ruc)
    {
        string path = Path.Combine(webHostEnvironment.ContentRootPath, "Files", ruc, "cert.p12");

        return File.Exists(path) ? path : null;
    }

    private string? BuscarXmlPorClaveDeAcceso(string claveAcceso)
    {
        if (string.IsNullOrWhiteSpace(claveAcceso) || claveAcceso.Length != 49)
        {
            throw new ArgumentException("❌ Clave de acceso inválida. Debe tener exactamente 49 caracteres numéricos.");
        }

        try
        {
            string path = ConstruirRutaXml(claveAcceso);

            return File.Exists(path) ? path : null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private string ConstruirRutaXml(string claveAcceso)
    {
        string ruc = claveAcceso.Substring(10, 13);
        string serie = claveAcceso.Substring(24, 6);
        string numeroComprobante = claveAcceso.Substring(30, 9);

        string establecimiento = serie.Substring(0, 3);
        string puntoEmision = serie.Substring(3, 3);

        string xmlDocName = $@"{establecimiento}_{puntoEmision}_{numeroComprobante}_{claveAcceso}.xml";

        return Path.Combine(webHostEnvironment.ContentRootPath, "Files", ruc, "DocumentosAutorizados",
            xmlDocName);
    }
}
