using FactElectFixed.Api.Features.Factura.Requests;
using FactElectFixed.Api.Features.NotaCredito.Requests;
using FactElectFixed.Api.Features.NotaCredito.Xml;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Responses;
using FactElectFixed.Api.Services.FirmarDocumento;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FactElectFixed.Api.Features.NotaCredito.Endpoints;

public class FirmarNotaCreditoEndpoint(IFirmarDocumentoService documentoService)
    : Endpoint<FirmarDocumentoRequest<NotaCreditoRequest>, Results<Ok<FirmarDocumentoResponse>, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/api/documentos/{Ambiente}/nota-credito/{Version}/firmar");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<FirmarDocumentoResponse>, ProblemDetails>> ExecuteAsync(
        FirmarDocumentoRequest<NotaCreditoRequest> req, CancellationToken ct)
    {
        FirmarDocumentoResponse enviarDocumentoSriResponse = await documentoService.EnviarDocumentoSri<NotaCreditoRequest, NotaCreditoXmlModel>(req);
        return TypedResults.Ok(enviarDocumentoSriResponse);
    }
}
