using FactElectFixed.Api.Requests;

namespace FactElectFixed.Api.Helpers.Interfaces;


public interface IDocumentoElectronicoBase<out T> : IXmlSerializable<T> where T : class
{
    InfoTributariaRequest InfoTributariaRequest { get; set; }
}

public interface IDocumentoElectronico<out T> : IDocumentoElectronicoBase<T> where T : class
{
    List<DetalleRequest> Detalles { get; set; }
}

public interface IDocumentoElectronicoNoDetalles<out T> : IDocumentoElectronicoBase<T> where T : class
{
}
