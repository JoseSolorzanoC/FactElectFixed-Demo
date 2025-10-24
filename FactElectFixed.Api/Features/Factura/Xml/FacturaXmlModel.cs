using System.Xml.Serialization;

namespace FactElectFixed.Api.Features.Factura.Xml;

[XmlRoot("factura", Namespace = "", IsNullable = false)]
public class FacturaXmlModel
{
    [XmlAttribute("id")] public string Id { get; set; } = "comprobante";

    [XmlAttribute("version")] public string Version { get; set; } = "2.1.0";

    [XmlElement("infoTributaria", Order = 1)]
    public InfoTributaria InfoTributaria { get; set; }

    [XmlElement("infoFactura", Order = 2)] public InfoFactura InfoFactura { get; set; }

    [XmlArray("detalles", Order = 3)]
    [XmlArrayItem("detalle")]
    public List<Detalle> Detalles { get; set; }

    [XmlArray("otrosRubrosTerceros", Order = 4)]
    [XmlArrayItem("rubro")]
    public List<Rubro>? OtrosRubrosTerceros { get; set; }
}

public class InfoTributaria
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

public class InfoFactura
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
    public List<TotalImpuesto> TotalConImpuestos { get; set; }

    [XmlElement("propina", Order = 12)] public decimal Propina { get; set; }

    [XmlElement("importeTotal", Order = 13)]
    public decimal ImporteTotal { get; set; }

    [XmlElement("moneda", Order = 14)] public string Moneda { get; set; }

    [XmlArray("pagos", Order = 15)]
    [XmlArrayItem("pago")]
    public List<Pago> Pagos { get; set; }

    [XmlElement("valorRetIva", Order = 16)]
    public decimal? ValorRetIva { get; set; }

    [XmlElement("valorRetRenta", Order = 17)]
    public decimal? ValorRetRenta { get; set; }
}

public class TotalImpuesto
{
    [XmlElement("codigo", Order = 1)] public int Codigo { get; set; }

    [XmlElement("codigoPorcentaje", Order = 2)]
    public int CodigoPorcentaje { get; set; }

    [XmlElement("baseImponible", Order = 3)]
    public decimal BaseImponible { get; set; }

    [XmlElement("valor", Order = 4)] public decimal Valor { get; set; }
}

public class Pago
{
    [XmlElement("formaPago", Order = 1)] public string FormaPago { get; set; }

    [XmlElement("total", Order = 2)] public decimal Total { get; set; }

    [XmlElement("plazo", Order = 3)] public decimal Plazo { get; set; }

    [XmlElement("unidadTiempo", Order = 4)]
    public string UnidadTiempo { get; set; }
}

public class Detalle
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
    public List<Impuesto> Impuestos { get; set; }
}

public class Impuesto
{
    [XmlElement("codigo", Order = 1)] public int Codigo { get; set; }

    [XmlElement("codigoPorcentaje", Order = 2)]
    public int CodigoPorcentaje { get; set; }

    [XmlElement("tarifa", Order = 3)] public decimal Tarifa { get; set; }

    [XmlElement("baseImponible", Order = 4)]
    public decimal BaseImponible { get; set; }

    [XmlElement("valor", Order = 5)] public decimal Valor { get; set; }
}

public class Rubro
{
    [XmlElement("concepto", Order = 1)] public string Concepto { get; set; }

    [XmlElement("total", Order = 2)] public decimal Total { get; set; }
}
