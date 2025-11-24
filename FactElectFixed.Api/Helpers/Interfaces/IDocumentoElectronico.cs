using Detalle = FactElectFixed.Api.Requests.Detalle;
using InfoTributaria = FactElectFixed.Api.Requests.InfoTributaria;

namespace FactElectFixed.Api.Helpers.Interfaces;


public interface IDocumentoElectronicoBase<out T> : IXmlSerializable<T> where T : class
{
    InfoTributaria InfoTributaria { get; set; }
}

public interface IDocumentoElectronico<out T> : IDocumentoElectronicoBase<T> where T : class
{
    List<Detalle> Detalles { get; set; }
}

public interface IDocumentoElectronicoNoDetalles<out T> : IDocumentoElectronicoBase<T> where T : class
{
}
