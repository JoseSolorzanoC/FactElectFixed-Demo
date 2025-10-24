namespace FactElectFixed.Api.Features.Factura.Reponses;

public class FirmarFacturaResponse
{
    public bool Success { get; set; }
    public string? ClaveAcceso { get; set; }
    public ComprobanteResponse Comprobante { get; set; }
}

public class ComprobanteResponse
{
    public bool Success { get; set; }
    public string? ClaveAcceso { get; set; }
    public string? EstadoAutorizacion { get; set; }
    public string? EstadoRecepcion { get; set; }
    public List<ErrorMensajeSri>? ErroresRecepcion { get; set; }
    public List<ErrorMensajeSri>? ErroresAutorizacion { get; set; }
    public string XmlProcesado { get; set; }
}

public class ErrorMensajeSri
{
    public int IndiceComprobante { get; set; }
    public string XmlProcesado { get; set; }
    public string? ClaveAcceso { get; set; }
    public string Codigo { get; set; }
    public string Mensaje { get; set; }
}
