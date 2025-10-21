using System.Xml;
using FactElectFixed.Api.Features.Factura.Firma.Reponses;
using FactElectFixed.Api.Features.Factura.Firma.Requests;

namespace FactElectFixed.Api.Features.Factura.Firma.Services.FirmarFactura;

public interface IFirmarFacturaService
{
    Task<EnviarFacturaSriResponse> EnviarFacturaSri(FirmarFacturaRequest firmarFacturaRequest);
    Task<VerificarFacturaResponse> VerificarFacturaSri(string claveAcceso, string xmlFacturaFirmado);
}
