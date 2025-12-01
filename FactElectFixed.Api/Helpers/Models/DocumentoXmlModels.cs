using System.Xml.Serialization;

namespace FactElectFixed.Api.Helpers.Models;

public class InfoTributariaXml
{
    [XmlElement("ambiente", Order = 1)] public int Ambiente { get; set; }

    [XmlElement("tipoEmision", Order = 2)] public int TipoEmision { get; set; }

    [XmlElement("razonSocial", Order = 3)] public string RazonSocial { get; set; }

    [XmlElement("nombreComercial", Order = 4)]
    public string NombreComercial { get; set; }

    [XmlElement("ruc", Order = 5)] public string Ruc { get; set; }

    [XmlElement("claveAcceso", Order = 6)] public string ClaveAcceso { get; set; }

    [XmlElement("codDoc", Order = 7)] public string CodDoc { get; set; }

    [XmlElement("estab", Order = 8)] public string Estab { get; set; }

    [XmlElement("ptoEmi", Order = 9)] public string PtoEmi { get; set; }

    [XmlElement("secuencial", Order = 10)] public string Secuencial { get; set; }

    [XmlElement("dirMatriz", Order = 11)] public string DirMatriz { get; set; }
}

public class TotalImpuestoXml
{
    [XmlElement("codigo", Order = 1)] public int Codigo { get; set; }

    [XmlElement("codigoPorcentaje", Order = 2)]
    public int CodigoPorcentaje { get; set; }

    [XmlElement("baseImponible", Order = 3)]
    public decimal BaseImponible { get; set; }

    [XmlElement("valor", Order = 4)] public decimal Valor { get; set; }
}

public class PagoXml
{
    [XmlElement("formaPago", Order = 1)] public string FormaPago { get; set; }

    [XmlElement("total", Order = 2)] public decimal Total { get; set; }

    [XmlElement("plazo", Order = 3)] public decimal Plazo { get; set; }

    [XmlElement("unidadTiempo", Order = 4)]
    public string UnidadTiempo { get; set; }
}

public class DetalleXml
{
    [XmlElement("codigoPrincipal", Order = 1)]
    public string CodigoPrincipal { get; set; }

    [XmlElement("codigoAuxiliar", Order = 2)]
    public string CodigoAuxiliar { get; set; }

    [XmlElement("descripcion", Order = 3)] public string Descripcion { get; set; }

    [XmlElement("cantidad", Order = 4)] public decimal Cantidad { get; set; }

    [XmlElement("precioUnitario", Order = 5)]
    public decimal PrecioUnitario { get; set; }

    [XmlElement("descuento", Order = 6)] public decimal Descuento { get; set; }

    [XmlElement("precioTotalSinImpuesto", Order = 7)]
    public decimal PrecioTotalSinImpuesto { get; set; }

    [XmlArray("impuestos", Order = 8)]
    [XmlArrayItem("impuesto")]
    public List<ImpuestoXml> Impuestos { get; set; }
}

public class ImpuestoXml
{
    [XmlElement("codigo", Order = 1)] public int Codigo { get; set; }

    [XmlElement("codigoPorcentaje", Order = 2)]
    public int CodigoPorcentaje { get; set; }

    [XmlElement("tarifa", Order = 3)] public decimal Tarifa { get; set; }

    [XmlElement("baseImponible", Order = 4)]
    public decimal BaseImponible { get; set; }

    [XmlElement("valor", Order = 5)] public decimal Valor { get; set; }
}

public class RubroXml
{
    [XmlElement("concepto", Order = 1)] public string Concepto { get; set; }

    [XmlElement("total", Order = 2)] public decimal Total { get; set; }
}

public class CampoAdicionalXml
{
    [XmlAttribute("nombre")]
    public string Nombre { get; set; }

    [XmlText]
    public string Valor { get; set; }
}

public interface IDocumentoXmlModel
{
    InfoTributariaXml InfoTributariaXml { get; set; }
}
