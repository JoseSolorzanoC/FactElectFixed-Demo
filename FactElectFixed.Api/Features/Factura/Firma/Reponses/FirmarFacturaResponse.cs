using Infoware.SRI.WebService.Response;

namespace FactElectFixed.Api.Features.Factura.Firma.Reponses;

public class FirmarFacturaResponse
{
    public bool Success { get; set; }
    public string ClaveAcceso { get; set; }
    public string? XmlProcesado { get; set; }
    public List<ValidarComprobanteResponse.Mensaje> MensajesValidacionSri { get; set; } = [];
    public List<AutorizarComprobanteResponse.Mensaje> MensajesAutorizacionSri { get; set; } = [];
}
