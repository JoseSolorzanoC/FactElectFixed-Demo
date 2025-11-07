using System.Xml;
using System.Xml.Linq;
using FactElectFixed.Api.Database;
using FactElectFixed.Api.Features.Configuracion.Entities;
using FactElectFixed.Api.Features.Factura.Mappers;
using FactElectFixed.Api.Features.Factura.Reponses;
using FactElectFixed.Api.Features.Factura.Requests;
using FactElectFixed.Api.Features.Factura.Xml;
using FactElectFixed.Api.Helpers;
using FactElectFixed.Api.Services;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Core.Helpers;
using Infoware.SRI.Firmar;
using Infoware.SRI.WebService;
using Infoware.SRI.WebService.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using ZiggyCreatures.Caching.Fusion;

namespace FactElectFixed.Api.Features.Factura.Services.FirmarFactura;

public class FirmarFacturaService(
    ISRIWebService sriWebService,
    IWebHostEnvironment webHostEnvironment,
    ICachedXmlFileService cachedXmlFileService,
    ApplicationDbContext dbContext,
    IFusionCache cache)
    : IFirmarFacturaService
{
    private ISRIWebService _sriWebService = sriWebService;

    public async Task<FirmarFacturaResponse> EnviarFacturaSri(FirmarFacturaRequest firmarFacturaRequest)
    {
        try
        {
            _sriWebService = _sriWebService.Using(firmarFacturaRequest.Ambiente, EnumTipoEsquema.Offline);
            Tuple<XmlDocument, string?> tuplaSerializacion = await SerializarXml(firmarFacturaRequest);

            string? rutaXmlExistente = tuplaSerializacion.Item2;

            if (rutaXmlExistente is not null)
            {
                string claveAccesoExistente = rutaXmlExistente.Split("_")[3];
                return new FirmarFacturaResponse
                {
                    Success = true,
                    ClaveAcceso = claveAccesoExistente,
                    Comprobante = new ComprobanteResponse
                    {
                        ClaveAcceso = claveAccesoExistente,
                        EstadoAutorizacion = "AUTORIZADO",
                        EstadoRecepcion = "RECIBIDO",
                        Success = true,
                        XmlProcesados = ExtraerComprobantesDesdeLote(cachedXmlFileService.GetXml(rutaXmlExistente))
                    }
                };
            }

            XmlDocument xmlFactura = tuplaSerializacion.Item1;

            string? claveAcceso = xmlFactura.SelectSingleNode("//claveAcceso")?.InnerText.Trim();

            Response<ValidarComprobanteResponse.RespuestaRecepcionComprobante> respuesta =
                await _sriWebService.ValidarComprobanteAsync(xmlFactura);

            if (respuesta is { Ok: false, Data.Comprobantes.Comprobante: not null })
            {
                var mensajesSri = new List<ErrorMensajeSri>();
                int indiceComprobante = 0;

                foreach (ValidarComprobanteResponse.Comprobante errorSriComprobante in respuesta.Data.Comprobantes
                             .Comprobante)
                {
                    if (errorSriComprobante is { Mensajes.Mensaje: not null })
                    {
                        string factura = xmlFactura.SelectNodes("//comprobantes/comprobante")?[indiceComprobante]
                            ?.InnerText.Trim();

                        mensajesSri.AddRange(errorSriComprobante.Mensajes.Mensaje.Select(m => new ErrorMensajeSri
                        {
                            IndiceComprobante = indiceComprobante,
                            ClaveAcceso = errorSriComprobante.ClaveAcceso,
                            Mensaje = $"{m.Mensaje_} - {m.InformacionAdicional}",
                            XmlProcesado = factura ?? "NO DEFINIDO",
                            Codigo = $"[{m.Tipo}][{m.Identificador}]"
                        }));
                    }

                    indiceComprobante++;
                }

                return new FirmarFacturaResponse
                {
                    Success = false,
                    ClaveAcceso = claveAcceso,
                    Comprobante = new ComprobanteResponse
                    {
                        ErroresRecepcion = mensajesSri,
                        XmlProcesados = ExtraerComprobantesDesdeLote(xmlFactura.OuterXml),
                        EstadoRecepcion = respuesta.Data.Estado ?? "NO DEFINIDO",
                        Success = false,
                        ClaveAcceso = claveAcceso
                    }
                };
            }

            if (claveAcceso is not null)
            {
                return await VerificarFacturaSri(claveAcceso, xmlFactura.OuterXml, firmarFacturaRequest.Ambiente);
            }

            return new FirmarFacturaResponse
            {
                Success = false,
                ClaveAcceso = claveAcceso,
                Comprobante = new ComprobanteResponse
                {
                    ClaveAcceso = "NO DEFINIDA",
                    Success = false,
                    EstadoAutorizacion = "ERROR AL OBTENER CLAVE DE ACCESO",
                    EstadoRecepcion = "ERROR AL OBTENER CLAVE DE ACCESO",
                    XmlProcesados = ExtraerComprobantesDesdeLote(xmlFactura.OuterXml)
                }
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<FirmarFacturaResponse> VerificarFacturaSri(string claveAcceso, string xmlFacturaFirmado,
        EnumTipoAmbiente ambiente)
    {
        try
        {
            _sriWebService = _sriWebService.Using(ambiente, EnumTipoEsquema.Offline);
            Response<AutorizarComprobanteResponse.RespuestaAutorizacionComprobante> respuesta =
                await _sriWebService.AutorizacionComprobanteAsync(claveAcceso);

            if (respuesta is { Ok: false, Data.Autorizaciones.Autorizacion: not null })
            {
                var mensajesSri = new List<ErrorMensajeSri>();
                int indiceComprobante = 0;

                foreach (AutorizarComprobanteResponse.Autorizacion errorSriComprobante in respuesta.Data.Autorizaciones
                             .Autorizacion)
                {
                    if (errorSriComprobante is { Mensajes.Mensaje: not null })
                    {
                        mensajesSri.AddRange(errorSriComprobante.Mensajes.Mensaje.Select(m => new ErrorMensajeSri
                        {
                            IndiceComprobante = indiceComprobante,
                            ClaveAcceso = respuesta.Data.ClaveAccesoConsultada,
                            Mensaje = $"{m.Mensaje_} - {m.InformacionAdicional}",
                            XmlProcesado = errorSriComprobante.Comprobante ?? "NO ENCONTRADO",
                            Codigo = $"[{m.Identificador}][{m.Tipo}]"
                        }));
                    }

                    indiceComprobante++;
                }

                return new FirmarFacturaResponse
                {
                    Success = false,
                    ClaveAcceso = claveAcceso,
                    Comprobante = new ComprobanteResponse
                    {
                        ErroresAutorizacion = mensajesSri,
                        XmlProcesados = ExtraerComprobantesDesdeLote(xmlFacturaFirmado),
                        EstadoRecepcion = "RECIBIDA",
                        EstadoAutorizacion = "NO AUTORIZADO",
                        Success = false,
                        ClaveAcceso = claveAcceso
                    }
                };
            }

            if (respuesta.Ok)
            {
                SafeXmlFileWriter.SaveXml(ConstruirRutaXml(claveAcceso), xmlFacturaFirmado);

                return new FirmarFacturaResponse
                {
                    Success = true,
                    ClaveAcceso = claveAcceso,
                    Comprobante = new ComprobanteResponse
                    {
                        ClaveAcceso = claveAcceso,
                        XmlProcesados = ExtraerComprobantesDesdeLote(xmlFacturaFirmado),
                        EstadoRecepcion = "RECIBIDA",
                        Success = true,
                        EstadoAutorizacion = "AUTORIZADO"
                    }
                };
            }

            return new FirmarFacturaResponse
            {
                Success = false,
                ClaveAcceso = claveAcceso,
                Comprobante = new ComprobanteResponse
                {
                    ClaveAcceso = claveAcceso,
                    XmlProcesados = ExtraerComprobantesDesdeLote(xmlFacturaFirmado),
                    EstadoRecepcion = "RECIBIDO",
                    EstadoAutorizacion = "NO DEFINIDO - ERROR",
                    Success = false
                }
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<Tuple<XmlDocument, string?>> SerializarXml(FirmarFacturaRequest firmarFacturaRequest)
    {
        ArgumentNullException.ThrowIfNull(firmarFacturaRequest);

        var comprobantesElement = new XElement("comprobantes");

        string? claveAccesoPrimerComprobante = null;

        foreach (FacturaXmlModel facturaXmlModel in
                 firmarFacturaRequest.Comprobantes.Select(factura =>
                     factura.ToXml(firmarFacturaRequest.Ambiente, firmarFacturaRequest.Version)))
        {
            XmlDocument facturaFirmadaXml =
                await FirmarXml(facturaXmlModel.ToXmlDocument(), facturaXmlModel.InfoTributaria.Ruc);

            comprobantesElement.Add(new XElement("comprobante",
                new XCData(facturaFirmadaXml.OuterXml)));

            claveAccesoPrimerComprobante ??= facturaXmlModel.InfoTributaria.ClaveAcceso;
        }

        var lote = new XElement("lote",
            new XAttribute("version", "1.0.0"),
            new XElement("claveAcceso", claveAccesoPrimerComprobante!),
            new XElement("ruc", firmarFacturaRequest.Comprobantes[0].InfoTributaria.Ruc),
            comprobantesElement
        );

        var documentoLote = new XDocument(new XDeclaration("1.0", "UTF-8", null), lote);

        return new Tuple<XmlDocument, string?>(documentoLote.ToXmlDocument(),
            BuscarXmlPorClaveDeAcceso(claveAccesoPrimerComprobante!));
    }

    private async Task<XmlDocument> FirmarXml(XmlDocument xmlFactura, string ruc)
    {
        var certificadoService = new CertificadoService(new NullLogger<CertificadoService>());

        try
        {
            ConfiguracionEntity certificadoInfo = await ObtenerConfiguracionEmpresa(ruc);
            string? rutaCertificado = BuscarCertificadoPorRuc(ruc);

            if (rutaCertificado is null)
            {
                throw new Exception("No se pudo encontrar un certificado asociado al RUC de la empresa");
            }

            certificadoService.CargarDesdeP12(rutaCertificado,
                certificadoInfo.Password);

            return certificadoService.FirmarDocumento(xmlFactura);
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

    private async Task<ConfiguracionEntity> ObtenerConfiguracionEmpresa(string ruc, CancellationToken ct = default)
    {
        return await cache.GetOrSetAsync<ConfiguracionEntity>(ruc,
            _ => dbContext.Configuraciones.FirstOrDefaultAsync(config => config.RucEmpresa == ruc, ct)!, token: ct, duration: TimeSpan.FromHours(8));
    }

    private static List<ComprobanteExtraido> ExtraerComprobantesDesdeLote(string xmlLote)
    {
        var resultado = new List<ComprobanteExtraido>();

        try
        {
            var documento = XDocument.Parse(xmlLote);

            IEnumerable<XElement> comprobantes = documento.Descendants("comprobante");

            foreach (XElement comprobante in comprobantes)
            {
                foreach (XCData nodo in comprobante.Nodes().OfType<XCData>())
                {
                    string cdataLiteral = $"<![CDATA[{nodo.Value}]]>";

                    string claveAcceso = string.Empty;

                    try
                    {
                        var innerXml = XDocument.Parse(nodo.Value);
                        XElement? claveNode = innerXml.Descendants("claveAcceso").FirstOrDefault();
                        if (claveNode != null)
                        {
                            claveAcceso = claveNode.Value.Trim();
                        }
                    }
                    catch
                    {
                        // Si el contenido no es XML válido, simplemente se omite la clave
                    }

                    resultado.Add(new ComprobanteExtraido
                    {
                        ClaveAcceso = claveAcceso,
                        XmlCData = cdataLiteral
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al extraer comprobantes: {ex.Message}");
        }

        return resultado;
    }
}
