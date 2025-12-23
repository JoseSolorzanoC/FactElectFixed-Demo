using FactElectFixed.Api.Features.Retencion.Requests;
using FactElectFixed.Api.Features.Retencion.Xml;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Responses;
using FactElectFixed.Api.Services.FirmarDocumento;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FactElectFixed.Api.Features.Retencion.Endpoints;

public class FirmarRetencionEndpoint(IFirmarDocumentoService documentoService)
    : Endpoint<FirmarDocumentoRequest<RetencionRequest>, Results<Ok<FirmarDocumentoResponse>, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/api/documentos/{Ambiente}/retencion/{Version}/firmar");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<FirmarDocumentoResponse>, ProblemDetails>> ExecuteAsync(
        FirmarDocumentoRequest<RetencionRequest> req, CancellationToken ct)
    {
        FirmarDocumentoResponse enviarDocumentoSriResponse =
            await documentoService.EnviarDocumentoSri<RetencionRequest, RetencionXmlModel>(req, ct);
        return TypedResults.Ok(enviarDocumentoSriResponse);
    }
}
