using FactElectFixed.Api.Features.Configuracion.Entities;
using FactElectFixed.Api.Features.Configuracion.Requests;
using FactElectFixed.Api.Features.Configuracion.Responses;
using FactElectFixed.Api.Features.Configuracion.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using ConfiguracionEntity = FactElectFixed.Api.Features.Configuracion.Responses.ConfiguracionEntity;

namespace FactElectFixed.Api.Features.Configuracion.Endpoints;

public class AgregarConfiguracionEndpoint(IConfiguracionService configuracionService)
    : Endpoint<AgregarConfiguracionRequest, Results<Ok<AgregarConfiguracionResponse>, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/api/configuracion/provisionar");
        AllowAnonymous();
        AllowFileUploads();
    }

    public override async Task<Results<Ok<AgregarConfiguracionResponse>, ProblemDetails>> ExecuteAsync(AgregarConfiguracionRequest req,
        CancellationToken ct)
    {
        try
        {
            Entities.ConfiguracionEntity entity =
                await configuracionService.SaveConfiguracion(req.RucEmpresa, req.Password, req.ArchivoFirmaP12, ct);

            return TypedResults.Ok(new AgregarConfiguracionResponse
            {
                Success = true,
                Respuesta = "Configuracion Agregada Correctamente",
                Configuracion = new ConfiguracionEntity(entity.Id, entity.RucEmpresa, entity.Password)
            });
        }
        catch (Exception e)
        {
            return new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = e.Message,
            };
        }
    }
}
