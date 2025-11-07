using FactElectFixed.Api.Features.Factura.Reponses;
using FactElectFixed.Api.Features.Factura.Requests;
using Infoware.SRI.Core.Enumerados;

namespace FactElectFixed.Api.Features.Factura.Services.FirmarFactura;

public interface IFirmarFacturaService
{
    Task<FirmarFacturaResponse> EnviarFacturaSri(FirmarFacturaRequest firmarFacturaRequest);
    Task<FirmarFacturaResponse> VerificarFacturaSri(string claveAcceso, string xmlFacturaFirmado, EnumTipoAmbiente ambiente, List<ComprobanteResponse> comprobantesYaAutorizados);
}
