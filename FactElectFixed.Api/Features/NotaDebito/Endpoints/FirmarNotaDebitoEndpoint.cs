using FactElectFixed.Api.Features.NotaDebito.Requests;
using FactElectFixed.Api.Features.NotaDebito.Xml;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Responses;
using FactElectFixed.Api.Services.FirmarDocumento;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FactElectFixed.Api.Features.NotaDebito.Endpoints;

public class FirmarNotaDebitoEndpoint(IFirmarDocumentoService documentoService)
    : Endpoint<FirmarDocumentoRequest<NotaDebitoRequest>, Results<Ok<FirmarDocumentoResponse>, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/api/documentos/{Ambiente}/nota-debito/{Version}/firmar");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<FirmarDocumentoResponse>, ProblemDetails>> ExecuteAsync(
        FirmarDocumentoRequest<NotaDebitoRequest> req, CancellationToken ct)
    {
        FirmarDocumentoResponse enviarDocumentoSriResponse =
            await documentoService.EnviarDocumentoSri<NotaDebitoRequest, NotaDebitoXmlModel>(req);
        return TypedResults.Ok(enviarDocumentoSriResponse);
    }
}
