using FactElectFixed.Api.Features.Factura.Firma.Reponses;
using FactElectFixed.Api.Features.Factura.Firma.Requests;
using FactElectFixed.Api.Features.Factura.Firma.Services.FirmarFactura;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FactElectFixed.Api.Features.Factura.Firma.Endpoints;

public class FirmarFacturaEndpoint(IFirmarFacturaService facturaService) : Endpoint<FirmarFacturaRequest, Results<Ok<FirmarFacturaResponse>, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/api/documentos/{Ambiente}/factura/{Version}/firmar");
        AllowAnonymous();
    }
    
    public override async Task<Results<Ok<FirmarFacturaResponse>, ProblemDetails>> ExecuteAsync(FirmarFacturaRequest req, CancellationToken ct)
    {
        EnviarFacturaSriResponse enviarFacturaSriResponse = await facturaService.EnviarFacturaSri(req);

        return TypedResults.Ok(new FirmarFacturaResponse
        {
            Success = enviarFacturaSriResponse.Success,
            ClaveAcceso = "",
            MensajeRespuesta = [.. enviarFacturaSriResponse.MensajesSriAutorizacion?.Select(m => $"[{m.Tipo}][{m.Identificador}] - {m.Mensaje_}: {m.InformacionAdicional}")!],
            XmlProcesado = enviarFacturaSriResponse.XmlProcesado
        });
    }
}
