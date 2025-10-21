using Infoware.SRI.WebService.Response;

namespace FactElectFixed.Api.Features.Factura.Firma.Reponses;

public class VerificarFacturaResponse
{
    public bool Success { get; set; }
    public string Respuesta { get; set; } = string.Empty;
    public string ClaveAcceso { get; set; } = string.Empty;
    public string XmlProcesado  { get; set; } = string.Empty;
    public List<AutorizarComprobanteResponse.Mensaje>? MensajesSri { get; set; }
}
