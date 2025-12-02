using System.Xml.Serialization;
using FactElectFixed.Api.Helpers.Models;

namespace FactElectFixed.Api.Features.Factura.Xml;

[XmlRoot("factura", Namespace = "", IsNullable = false)]
public class FacturaXmlModel : IDocumentoXmlModel
{
    [XmlAttribute("id")] public string Id { get; set; } = "comprobante";

    [XmlAttribute("version")] public string Version { get; set; } = "2.1.0";

    [XmlElement("infoFactura", Order = 2)] public InfoFacturaXml InfoFacturaXml { get; set; }

    [XmlArray("detalles", Order = 3)]
    [XmlArrayItem("detalle")]
    public List<DetalleXml> Detalles { get; set; }

    [XmlArray("otrosRubrosTerceros", Order = 4)]
    [XmlArrayItem("rubro")]
    public List<RubroXml>? OtrosRubrosTerceros { get; set; }

    [XmlElement("infoTributaria", Order = 1)]
    public InfoTributariaXml InfoTributariaXml { get; set; }
}

public class InfoFacturaXml
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

    [XmlElement("contribuyenteEspecial", Order = 3)]
    public string ContribuyenteEspecial { get; set; }

    [XmlElement("obligadoContabilidad", Order = 4)]
    public string ObligadoContabilidadString
    {
        get => ObligadoContabilidad ? "SI" : "NO";
        set => ObligadoContabilidad = value == "SI";
    }

    [XmlIgnore] public bool ObligadoContabilidad { get; set; }

    [XmlElement("tipoIdentificacionComprador", Order = 5)]
    public string TipoIdentificacionComprador { get; set; }

    [XmlElement("razonSocialComprador", Order = 6)]
    public string RazonSocialComprador { get; set; }

    [XmlElement("identificacionComprador", Order = 7)]
    public string IdentificacionComprador { get; set; }

    [XmlElement("direccionComprador", Order = 8)]
    public string DireccionComprador { get; set; }

    [XmlElement("totalSinImpuestos", Order = 9)]
    public decimal TotalSinImpuestos { get; set; }

    [XmlElement("totalDescuento", Order = 10)]
    public decimal TotalDescuento { get; set; }

    [XmlArray("totalConImpuestos", Order = 11)]
    [XmlArrayItem("totalImpuesto")]
    public List<TotalImpuestoXml> TotalConImpuestos { get; set; }

    [XmlElement("propina", Order = 12)] public decimal Propina { get; set; }

    [XmlElement("importeTotal", Order = 13)]
    public decimal ImporteTotal { get; set; }

    [XmlElement("moneda", Order = 14)] public string Moneda { get; set; }

    [XmlArray("pagos", Order = 15)]
    [XmlArrayItem("pago")]
    public List<PagoXml> Pagos { get; set; }

    [XmlElement("valorRetIva", Order = 16)]
    public decimal? ValorRetIva { get; set; }

    [XmlElement("valorRetRenta", Order = 17)]
    public decimal? ValorRetRenta { get; set; }
}
