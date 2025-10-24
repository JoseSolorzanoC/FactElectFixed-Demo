namespace FactElectFixed.Api.Features.Configuracion.Requests;

public class AgregarConfiguracionRequest
{
   public string RucEmpresa { get; set; } 
   public string Password { get; set; }
   public IFormFile ArchivoFirmaP12 { get; set; }
}
