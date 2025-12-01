using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Helpers.Models;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Responses;
using Infoware.SRI.Core.Enumerados;

namespace FactElectFixed.Api.Services.FirmarDocumento;

public interface IFirmarDocumentoService
{
    Task<FirmarDocumentoResponse> EnviarDocumentoSri<TRequest, TXmlModel>(FirmarDocumentoRequest<TRequest> firmarDocumentoRequest) where TRequest : IDocumentoElectronicoBase<TXmlModel> where TXmlModel : class, IDocumentoXmlModel;
    Task<FirmarDocumentoResponse> VerificarDocumentoSri(string claveAcceso, string xmlDocumentoFirmado, EnumTipoAmbiente ambiente, List<ComprobanteResponse> comprobantesYaAutorizados);
}
