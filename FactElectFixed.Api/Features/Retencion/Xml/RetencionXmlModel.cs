using System.Xml.Serialization;
using FactElectFixed.Api.Helpers.Models;

namespace FactElectFixed.Api.Features.Retencion.Xml;

[XmlRoot("comprobanteRetencion")]
public class RetencionXmlModel : IDocumentoXmlModel
{
    [XmlAttribute("id")] public string Id { get; set; } = "comprobante";

    [XmlAttribute("version")] public string Version { get; set; }

    [XmlElement("infoTributaria", Order = 2)]
    public InfoTributariaXml InfoTributariaXml { get; set; }

    [XmlElement("infoCompRetencion", Order = 3)]
    public InfoCompRetencionXml InfoCompRetencion { get; set; }

    [XmlArray("impuestos", Order = 4)]
    [XmlArrayItem("impuesto")]
    public List<ImpuestoRetencionXml> Impuestos { get; set; }

    [XmlArray("infoAdicional", Order = 5)]
    [XmlArrayItem("campoAdicional")]
    public List<CampoAdicionalXml> InfoAdicional { get; set; }
}

public class InfoCompRetencionXml
{
    [XmlElement("fechaEmision", Order = 1)]
    public string FechaEmisionString
    {
#pragma warning disable CA1305
        get => FechaEmision.ToString("dd/MM/yyyy");
#pragma warning restore CA1305
#pragma warning disable S6580
        set => FechaEmision = DateTime.ParseExact(value, "dd/MM/yyyy", null);
#pragma warning restore S6580
    }

    [XmlIgnore] public DateTime FechaEmision { get; set; }

    [XmlElement("dirEstablecimiento", Order = 2)]
    public string DirEstablecimiento { get; set; }

    [XmlElement("contribuyenteEspecial", Order = 3)]
    public string ContribuyenteEspecial { get; set; }

    [XmlElement("obligadoContabilidad", Order = 4)]
    public string ObligadoContabilidadString
    {
        get => ObligadoContabilidad ? "SI" : "NO";
        set => ObligadoContabilidad = value == "SI";
    }

    [XmlIgnore] public bool ObligadoContabilidad { get; set; }

    [XmlElement("tipoIdentificacionSujetoRetenido", Order = 5)]
    public string TipoIdentificacionSujetoRetenido { get; set; }
    [XmlElement("razonSocialSujetoRetenido", Order = 8)]
    public string RazonSocialSujetoRetenido { get; set; }

    [XmlElement("identificacionSujetoRetenido", Order = 9)]
    public string IdentificacionSujetoRetenido { get; set; }

    [XmlElement("periodoFiscal", Order = 10)]
    public string PeriodoFiscal { get; set; } 
}
public class ImpuestoRetencionXml
{
    [XmlElement("codigo")]
    public int Codigo { get; set; }
    [XmlElement("codigoRetencion")]
    public string CodigoRetencion { get; set; } 
    [XmlElement("baseImponible")]
    public decimal BaseImponible { get; set; }
    [XmlElement("porcentajeRetener")]
    public decimal PorcentajeRetener { get; set; } 
    [XmlElement("valorRetenido")]
    public decimal ValorRetenido { get; set; }
    [XmlElement("codDocSustento")]
    public string CodDocSustento { get; set; } 
    [XmlElement("numDocSustento")]
    public string NumDocSustento { get; set; } 
    [XmlElement("fechaEmisionDocSustento")]
    public string FechaEmisionDocSustento { get; set; } 
}

