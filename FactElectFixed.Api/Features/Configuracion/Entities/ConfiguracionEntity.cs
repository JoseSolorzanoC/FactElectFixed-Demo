using Microsoft.EntityFrameworkCore;

namespace FactElectFixed.Api.Features.Configuracion.Entities;

[Index(nameof(RucEmpresa), Name = "RucEmpresa", IsUnique = true)]
public class ConfiguracionEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string RucEmpresa { get; set; }
    public string Password { get; set; }
}
