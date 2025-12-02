using FactElectFixed.Api.Features.Factura.Requests;
using FactElectFixed.Api.Features.Factura.Xml;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Responses;
using FactElectFixed.Api.Services.FirmarDocumento;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FactElectFixed.Api.Features.Factura.Endpoints;

public class FirmarFacturaEndpoint(IFirmarDocumentoService documentoService)
    : Endpoint<FirmarDocumentoRequest<FacturaRequest>, Results<Ok<FirmarDocumentoResponse>, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/api/documentos/{Ambiente}/factura/{Version}/firmar");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<FirmarDocumentoResponse>, ProblemDetails>> ExecuteAsync(
        FirmarDocumentoRequest<FacturaRequest> req, CancellationToken ct)
    {
        FirmarDocumentoResponse enviarDocumentoSriResponse =
            await documentoService.EnviarDocumentoSri<FacturaRequest, FacturaXmlModel>(req);
        return TypedResults.Ok(enviarDocumentoSriResponse);
    }
}
