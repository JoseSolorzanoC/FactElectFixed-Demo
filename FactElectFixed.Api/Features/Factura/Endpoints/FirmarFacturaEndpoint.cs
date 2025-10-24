using FactElectFixed.Api.Features.Factura.Reponses;
using FactElectFixed.Api.Features.Factura.Requests;
using FactElectFixed.Api.Features.Factura.Services.FirmarFactura;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FactElectFixed.Api.Features.Factura.Endpoints;

public class FirmarFacturaEndpoint(IFirmarFacturaService facturaService)
    : Endpoint<FirmarFacturaRequest, Results<Ok<FirmarFacturaResponse>, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/api/documentos/{Ambiente}/factura/{Version}/firmar");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<FirmarFacturaResponse>, ProblemDetails>> ExecuteAsync(
        FirmarFacturaRequest req, CancellationToken ct)
    {
        FirmarFacturaResponse enviarFacturaSriResponse = await facturaService.EnviarFacturaSri(req);
        return TypedResults.Ok(enviarFacturaSriResponse);
    }
}
