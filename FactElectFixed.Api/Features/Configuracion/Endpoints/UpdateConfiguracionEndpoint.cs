using FactElectFixed.Api.Features.Configuracion.Requests;
using FactElectFixed.Api.Features.Configuracion.Responses;
using FactElectFixed.Api.Features.Configuracion.Services;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using ConfiguracionEntity = FactElectFixed.Api.Features.Configuracion.Entities.ConfiguracionEntity;

namespace FactElectFixed.Api.Features.Configuracion.Endpoints;

public class UpdateConfiguracionEndpoint(IConfiguracionService configuracionService)
    : Endpoint<CreateUpdateConfiguracionRequest, Results<Ok<CreateUpdateConfiguracionResponse>, ProblemDetails>>
{
    public override void Configure()
    {
        Patch("/api/configuracion/password/actualizar");
        AllowAnonymous();
        AllowFileUploads();
    }

    public override async Task<Results<Ok<CreateUpdateConfiguracionResponse>, ProblemDetails>> ExecuteAsync(
        CreateUpdateConfiguracionRequest req,
        CancellationToken ct)
    {
        try
        {
            ConfiguracionEntity entity =
                await configuracionService.UpdateConfiguracion(req.RucEmpresa, req.Password, req.ArchivoFirmaP12, ct);

            return TypedResults.Ok(new CreateUpdateConfiguracionResponse
            {
                Success = true,
                Respuesta = "Configuracion Actualizada Correctamente",
                Configuracion = new Responses.ConfiguracionEntity(entity.Id, entity.RucEmpresa, entity.Password)
            });
        }
        catch (Exception e)
        {
            return new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = e.Message
            };
        }
    }
}
