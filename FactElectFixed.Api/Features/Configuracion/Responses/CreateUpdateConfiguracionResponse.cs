namespace FactElectFixed.Api.Features.Configuracion.Responses;

public class CreateUpdateConfiguracionResponse
{
    public bool Success { get; set; }
    public string Respuesta { get; set; }
    public ConfiguracionEntity Configuracion { get; set; }
}

public record ConfiguracionEntity(string Id, string RucEmpresa, string Password);
