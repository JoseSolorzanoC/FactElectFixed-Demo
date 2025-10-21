using Infoware.SRI.WebService.Response;

namespace FactElectFixed.Api.Features.Factura.Firma.Reponses;

public class EnviarFacturaSriResponse
{
    public bool Success { get; set; }
    public List<ValidarComprobanteResponse.Mensaje> MensajesSriRecepcion { get; set; }
    public List<AutorizarComprobanteResponse.Mensaje>? MensajesSriAutorizacion { get; set; }
    public string XmlProcesado { get; set; } = string.Empty;
    public string ClaveAcceso { get; set; } = string.Empty;
}
