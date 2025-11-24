using Infoware.SRI.Core.Enumerados;

namespace FactElectFixed.Api.Helpers.Interfaces;

public interface IXmlSerializable<out TResult>
{
   TResult ToXml(EnumTipoAmbiente tipoAmbiente, string version); 
}
