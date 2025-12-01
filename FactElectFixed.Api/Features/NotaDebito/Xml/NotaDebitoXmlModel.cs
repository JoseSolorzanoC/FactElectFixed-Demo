using System.Xml.Serialization;
using FactElectFixed.Api.Helpers.Models;

namespace FactElectFixed.Api.Features.NotaDebito.Xml;

[XmlRoot("notaDebito")]
public class NotaDebitoXmlModel : IDocumentoXmlModel
{
    [XmlAttribute("version")]
    public string Version { get; set; }

    [XmlAttribute("id")]
    public string Id { get; set; } = "comprobante";

    [XmlElement("infoTributaria", Order = 1)]
    public InfoTributariaXml InfoTributariaXml { get; set; }

    [XmlElement("infoNotaDebito", Order = 2)]
    public InfoNotaDebitoXml InfoNotaDebitoXml { get; set; }

    [XmlArray("motivos", Order = 3)]
    [XmlArrayItem("motivo")]
    public List<Requests.MotivoRequest>? Motivos { get; set; }

    [XmlArray("infoAdicional", Order = 4)]
    [XmlArrayItem("campoAdicional")]
    public List<CampoAdicionalXml>? InfoAdicional { get; set; }
}

public class MotivoXml
{
    [XmlElement("razon", Order = 1)]
    public string Razon { get; set; }

    [XmlElement("valor", Order = 2)]
    public decimal Valor { get; set; }
}

public class InfoNotaDebitoXml
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

    [XmlElement("tipoIdentificacionComprador", Order = 3)]
    public string TipoIdentificacionComprador { get; set; }

    [XmlElement("razonSocialComprador", Order = 4)]
    public string RazonSocialComprador { get; set; }

    [XmlElement("identificacionComprador", Order = 5)]
    public string IdentificacionComprador { get; set; }

    [XmlElement("contribuyenteEspecial", Order = 6)]
    public string ContribuyenteEspecial { get; set; }

    [XmlElement("obligadoContabilidad", Order = 7)]
    public string ObligadoContabilidadString
    {
        get => ObligadoContabilidad ? "SI" : "NO";
        set => ObligadoContabilidad = value == "SI";
    }
    [XmlIgnore] public bool ObligadoContabilidad { get; set; }

    [XmlElement("codDocModificado", Order = 8)]
    public string CodDocModificado { get; set; }

    [XmlElement("numDocModificado", Order = 9)]
    public string NumDocModificado { get; set; }

    [XmlElement("fechaEmisionDocSustento", Order = 10)]
    public string FechaEmisionDocSustento { get; set; }

    [XmlElement("totalSinImpuestos", Order = 11)]
    public decimal TotalSinImpuestos { get; set; }

    [XmlArray("impuestos", Order = 12)]
    [XmlArrayItem("impuesto")]
    public List<ImpuestoXml>? Impuestos { get; set; }

    [XmlElement("valorTotal", Order = 13)]
    public decimal ValorTotal { get; set; }

    [XmlArray("pagos", Order = 14)]
    [XmlArrayItem("pago")]
    public List<PagoXml>? Pagos { get; set; }
}
