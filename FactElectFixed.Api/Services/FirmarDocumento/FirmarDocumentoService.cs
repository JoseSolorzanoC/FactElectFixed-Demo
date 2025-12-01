using System.Xml;
using System.Xml.Linq;
using FactElectFixed.Api.Database;
using FactElectFixed.Api.Features.Configuracion.Entities;
using FactElectFixed.Api.Features.Factura.Xml;
using FactElectFixed.Api.Helpers;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Helpers.Models;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Responses;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Core.Helpers;
using Infoware.SRI.Firmar;
using Infoware.SRI.LeerXml.Extensions;
using Infoware.SRI.Modelos;
using Infoware.SRI.WebService;
using Infoware.SRI.WebService.Response;
using Infoware.SRI.XSDs.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using ZiggyCreatures.Caching.Fusion;

namespace FactElectFixed.Api.Services.FirmarDocumento;

public class FirmarDocumentoService(
    ISRIWebService sriWebService,
    IWebHostEnvironment webHostEnvironment,
    ICachedXmlFileService cachedXmlFileService,
    ApplicationDbContext dbContext,
    IFusionCache cache)
    : IFirmarDocumentoService
{
    private ISRIWebService _sriWebService = sriWebService;

    public async Task<FirmarDocumentoResponse>
        EnviarDocumentoSri<TRequest, TXmlModel>(FirmarDocumentoRequest<TRequest> firmarDocumentoRequest)
        where TRequest : IDocumentoElectronicoBase<TXmlModel> where TXmlModel : class, IDocumentoXmlModel
    {
        try
        {
            int indiceComprobante = 0;

            _sriWebService = _sriWebService.Using(firmarDocumentoRequest.Ambiente, EnumTipoEsquema.Offline);
            XmlDocument tuplaSerializacion = await SerializarXml<TRequest, TXmlModel>(firmarDocumentoRequest);

            XmlDocument xmlFactura = tuplaSerializacion;

            var comprobantesAutorizados = new List<ComprobanteResponse>();

            foreach (TRequest _ in firmarDocumentoRequest.Comprobantes)
            {
                string claveAccesoComprobanteExistente =
                    xmlFactura.SelectNodes("//comprobantes/comprobante")?[indiceComprobante]
                        ?.SelectSingleNode("//claveAcceso")!.InnerText.Trim()!;

                string? rutaXmlExistente = BuscarXmlPorClaveDeAcceso(claveAccesoComprobanteExistente);

                if (rutaXmlExistente is not null)
                {
                    comprobantesAutorizados.Add(new ComprobanteResponse
                    {
                        Success = true,
                        XmlProcesado = cachedXmlFileService.GetXml(rutaXmlExistente),
                        ClaveAcceso = claveAccesoComprobanteExistente,
                        EstadoRecepcion = "RECIBIDA",
                        EstadoAutorizacion = "AUTORIZADA"
                    });
                }

                indiceComprobante++;
            }
            
            string? claveAcceso = xmlFactura.SelectSingleNode("//claveAcceso")?.InnerText.Trim();

            Response<ValidarComprobanteResponse.RespuestaRecepcionComprobante> respuesta =
                await _sriWebService.ValidarComprobanteAsync(xmlFactura);

            indiceComprobante = 0;

            if (respuesta is { Data.Comprobantes.Comprobante: not null })
            {
                var mensajesSri = new List<ErrorMensajeSri>();

                foreach (ValidarComprobanteResponse.Comprobante comprobanteSri in respuesta.Data.Comprobantes
                             .Comprobante)
                {
                    string factura = xmlFactura.SelectNodes("//comprobantes/comprobante")?[indiceComprobante]
                        ?.InnerText.Trim();

                    ComprobanteResponse comprobante = new()
                    {
                        ClaveAcceso = comprobanteSri.ClaveAcceso,
                        EstadoRecepcion = respuesta.Data.Estado,
                        XmlProcesado = factura,
                        Success = true
                    };

                    if (!respuesta.Ok && comprobanteSri is { Mensajes.Mensaje: not null })
                    {
                        mensajesSri.AddRange(comprobanteSri.Mensajes.Mensaje.Select(m => new ErrorMensajeSri
                        {
                            Mensaje = $"{m.Mensaje_} - {m.InformacionAdicional}",
                            Codigo = $"[{m.Tipo}][{m.Identificador}]"
                        }));
                    }

                    if (mensajesSri.Any())
                    {
                        comprobante.Success = false;
                        comprobante.ErroresAutorizacion = mensajesSri;
                    }

                    comprobantesAutorizados.Add(comprobante);
                    indiceComprobante++;
                }
            }

            if (claveAcceso is not null)
            {
                await Task.Delay(300);

                return await VerificarDocumentoSri(claveAcceso, xmlFactura.OuterXml, firmarDocumentoRequest.Ambiente,
                    comprobantesAutorizados);
            }

            return new FirmarDocumentoResponse
            {
                Success = false,
                ClaveAcceso = claveAcceso,
                Comprobantes =
                [
                    .. ExtraerComprobantesDesdeLote(xmlFactura.OuterXml).Select(extraido => new ComprobanteResponse
                    {
                        Success = false,
                        XmlProcesado = extraido.XmlCData,
                        ClaveAcceso = extraido.ClaveAcceso,
                        EstadoAutorizacion = "NO DEFINIDO",
                        EstadoRecepcion = "NO DEFINIDO"
                    })
                ]
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<FirmarDocumentoResponse> VerificarDocumentoSri(string claveAcceso, string xmlDocumentoFirmado,
        EnumTipoAmbiente ambiente, List<ComprobanteResponse> comprobantesYaAutorizados)
    {
        try
        {
            _sriWebService = _sriWebService.Using(ambiente, EnumTipoEsquema.Offline);
            Response<AutorizarComprobanteResponse.RespuestaAutorizacionComprobante> respuesta =
                await _sriWebService.AutorizacionComprobanteAsync(claveAcceso);

            var comprobantes = new List<ComprobanteResponse>(comprobantesYaAutorizados);

            if (respuesta is { Data.Autorizaciones.Autorizacion: not null })
            {
                var mensajesSri = new List<ErrorMensajeSri>();

                foreach (AutorizarComprobanteResponse.Autorizacion sriComprobante in respuesta.Data.Autorizaciones
                             .Autorizacion)
                {
                    ComprobanteResponse comprobante = new()
                    {
                        ClaveAcceso = ExtraerClaveAccesoString(sriComprobante.Comprobante ?? ""),
                        EstadoAutorizacion = sriComprobante.Estado,
                        EstadoRecepcion = "RECIBIDA",
                        XmlProcesado = sriComprobante.Comprobante,
                        Success = true
                    };

                    if (!respuesta.Ok && sriComprobante is { Mensajes.Mensaje: not null })
                    {
                        mensajesSri.AddRange(sriComprobante.Mensajes.Mensaje.Select(m => new ErrorMensajeSri
                        {
                            Mensaje = $"{m.Mensaje_} - {m.InformacionAdicional}",
                            Codigo = $"[{m.Identificador}][{m.Tipo}]"
                        }));
                    }

                    if (mensajesSri.Any())
                    {
                        comprobante.Success = false;
                        comprobante.ErroresAutorizacion = mensajesSri;
                    }

                    comprobantes.Add(comprobante);

                    if (comprobante is
                        { ClaveAcceso: not null, XmlProcesado: not null, EstadoAutorizacion: "AUTORIZADO" })
                    {
                        SafeXmlFileWriter.SaveXml(ConstruirRutaXml(comprobante.ClaveAcceso), comprobante.XmlProcesado);
                    }
                }

                return new FirmarDocumentoResponse
                {
                    Success = false,
                    ClaveAcceso = claveAcceso,
                    Comprobantes = comprobantes
                };
            }

            return new FirmarDocumentoResponse
            {
                Success = false,
                ClaveAcceso = claveAcceso,
                Comprobantes =
                [
                    .. ExtraerComprobantesDesdeLote(xmlDocumentoFirmado).Select(extraido => new ComprobanteResponse
                    {
                        Success = false,
                        XmlProcesado = extraido.XmlCData,
                        ClaveAcceso = extraido.ClaveAcceso,
                        EstadoAutorizacion = "NO DEFINIDO",
                        EstadoRecepcion = "RECIBIDA"
                    })
                ]
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<XmlDocument>
        SerializarXml<TRequest, TXmlModel>(FirmarDocumentoRequest<TRequest> firmarDocumentoRequest)
        where TRequest : IDocumentoElectronicoBase<TXmlModel> where TXmlModel : class, IDocumentoXmlModel
    {
        ArgumentNullException.ThrowIfNull(firmarDocumentoRequest);

        var comprobantesElement = new XElement("comprobantes");

        string? claveAccesoPrimerComprobante = null;

        foreach (TXmlModel facturaXmlModel in
                 firmarDocumentoRequest.Comprobantes.Select<TRequest, object>(factura =>
                     factura.ToXml(firmarDocumentoRequest.Ambiente, firmarDocumentoRequest.Version)))
        {
            XmlDocument facturaFirmadaXml =
                await FirmarXml(facturaXmlModel.ToXmlDocument(), facturaXmlModel.InfoTributariaXml.Ruc);

            // if (BuscarXmlPorClaveDeAcceso(facturaXmlModel.InfoTributariaXml.ClaveAcceso) is not null)
            // {
            comprobantesElement.Add(new XElement("comprobante",
                new XCData(facturaFirmadaXml.OuterXml)));
            // }

            claveAccesoPrimerComprobante ??= facturaXmlModel.InfoTributariaXml.ClaveAcceso;
        }

        var lote = new XElement("lote",
            new XAttribute("version", "1.0.0"),
            new XElement("claveAcceso", claveAccesoPrimerComprobante!),
            new XElement("ruc", firmarDocumentoRequest.Comprobantes[0].InfoTributariaRequest.Ruc),
            comprobantesElement
        );

        var documentoLote = new XDocument(new XDeclaration("1.0", "UTF-8", null), lote);

        return documentoLote.ToXmlDocument();
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
            _ => dbContext.Configuraciones.FirstOrDefaultAsync(config => config.RucEmpresa == ruc, ct)!, token: ct,
            duration: TimeSpan.FromHours(8));
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

    private string ExtraerClaveAccesoString(string xml)
    {
        string startTag = "<claveAcceso>";
        string endTag = "</claveAcceso>";

        int startIndex = xml.IndexOf(startTag, StringComparison.Ordinal) + startTag.Length;
        int endIndex = xml.IndexOf(endTag, StringComparison.Ordinal);

        return xml.Substring(startIndex, endIndex - startIndex);
    }
}
