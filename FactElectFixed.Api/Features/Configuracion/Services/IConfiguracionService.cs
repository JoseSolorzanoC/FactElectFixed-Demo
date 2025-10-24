using FactElectFixed.Api.Features.Configuracion.Entities;

namespace FactElectFixed.Api.Features.Configuracion.Services;

public interface IConfiguracionService
{
    Task<ConfiguracionEntity> SaveConfiguracion(string ruc, string password, IFormFile archivoP12,
        CancellationToken ct = default);

    Task<ConfiguracionEntity> UpdateConfiguracion(string ruc, string password, IFormFile archivoP12,
        CancellationToken ct = default);
}
