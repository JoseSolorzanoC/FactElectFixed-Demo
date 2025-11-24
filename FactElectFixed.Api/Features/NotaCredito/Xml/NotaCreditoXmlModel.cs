using System.Xml.Serialization;
using FactElectFixed.Api.Helpers.Models;

namespace FactElectFixed.Api.Features.NotaCredito.Xml;

[XmlRoot("notaCredito", Namespace = "", IsNullable = false)]
public class NotaCreditoXmlModel
{
    [XmlAttribute("id")] public string Id { get; set; } = "comprobante";

    [XmlAttribute("version")] public string Version { get; set; } = "2.1.0";

    [XmlElement("infoTributaria", Order = 1)]
    public InfoTributaria InfoTributaria { get; set; }

    [XmlElement("infoNotaCredito", Order = 2)] public InfoNotaCredito InfoNotaCredito { get; set; }

    [XmlArray("detalles", Order = 3)]
    [XmlArrayItem("detalle")]
    public List<Api.Requests.Detalle> Detalles { get; set; }
}

public class InfoNotaCredito
{
    [XmlElement("fechaEmision", Order = 1)]
    public string FechaEmisionString
    {
#pragma warning disable CA1305
#pragma warning disable S6580
        get => FechaEmision.ToString("dd/MM/yyyy");
        set => FechaEmision = DateTime.ParseExact(value, "dd/MM/yyyy", null);
#pragma warning restore S6580
#pragma warning restore CA1305
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

    [XmlElement("rise", Order = 8)]
    public string Rise { get; set; }

    [XmlElement("codDocModificado", Order = 9)]
    public string CodDocModificado { get; set; }

    [XmlElement("numDocModificado", Order = 10)]
    public string NumDocModificado { get; set; }

    [XmlElement("fechaEmisionDocSustento", Order = 11)]
    public string FechaEmisionDocSustentoString
    {
#pragma warning disable CA1305
#pragma warning disable S6580
        get => FechaEmisionDocSustento.ToString("dd/MM/yyyy");
        set => FechaEmisionDocSustento = DateTime.ParseExact(value, "dd/MM/yyyy", null);
#pragma warning restore S6580
#pragma warning restore CA1305
    }

    [XmlIgnore] public DateTime FechaEmisionDocSustento { get; set; }

    [XmlElement("totalSinImpuestos", Order = 12)]
    public decimal TotalSinImpuestos { get; set; }

    [XmlElement("valorModificacion", Order = 13)]
    public decimal ValorModificacion { get; set; }

    [XmlElement("moneda", Order = 14)]
    public string Moneda { get; set; }

    [XmlArray("totalConImpuestos", Order = 15)]
    [XmlArrayItem("totalImpuesto")]
    public List<TotalImpuesto> TotalConImpuestos { get; set; } = new();

    [XmlElement("motivo", Order = 16)]
    public string Motivo { get; set; }
}
