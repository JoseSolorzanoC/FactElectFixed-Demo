using System.Xml.Serialization;
using FactElectFixed.Api.Helpers.Models;

namespace FactElectFixed.Api.Features.Retencion.Xml;

[XmlRoot("comprobanteRetencion")]
public class RetencionXmlModel : IDocumentoXmlModel
{
    [XmlAttribute("id")]
    public string Id { get; set; } = "comprobante";

    [XmlAttribute("version")]
    public string Version { get; set; }

    [XmlElement("infoTributaria", Order = 1)]
    public InfoTributariaXml InfoTributariaXml { get; set; }

    [XmlElement("infoCompRetencion", Order = 2)]
    public InfoCompRetencionXml InfoCompRetencion { get; set; }

    [XmlArray("docsSustento", Order = 3)]
    [XmlArrayItem("docSustento")]
    public List<DocSustentoXml>? DocsSustento { get; set; } = new();

    [XmlArray("infoAdicional", Order = 4)]
    [XmlArrayItem("campoAdicional")]
    public List<CampoAdicionalXml>? InfoAdicional { get; set; } = new();
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

    [XmlElement("tipoSujetoRetenido", Order = 6)]
    public string TipoSujetoRetenido { get; set; }

    [XmlElement("parteRel", Order = 7)]
    public string ParteRel { get; set; }

    [XmlElement("razonSocialSujetoRetenido", Order = 8)]
    public string RazonSocialSujetoRetenido { get; set; }

    [XmlElement("identificacionSujetoRetenido", Order = 9)]
    public string IdentificacionSujetoRetenido { get; set; }

    [XmlElement("periodoFiscal", Order = 10)]
    public string PeriodoFiscal { get; set; } // mm/yyyy
}

public class DocSustentoXml
{
    [XmlElement("codSustento", Order = 1)]
    public string CodSustento { get; set; }

    [XmlElement("codDocSustento", Order = 2)]
    public string CodDocSustento { get; set; }

    [XmlElement("numDocSustento", Order = 3)]
    public string NumDocSustento { get; set; }

    [XmlElement("fechaEmisionDocSustento", Order = 4)]
    public string FechaEmisionDocSustentoString
    {
#pragma warning disable CA1305
        get => FechaEmisionDocSustento.ToString("dd/MM/yyyy");
#pragma warning restore CA1305
#pragma warning disable S6580
        set => FechaEmisionDocSustento = DateTime.ParseExact(value, "dd/MM/yyyy", null);
#pragma warning restore S6580
    }
    [XmlIgnore] public DateTime FechaEmisionDocSustento { get; set; }

    [XmlElement("fechaRegistroContable", Order = 5)]
    public string FechaRegistroContableString
    {
#pragma warning disable CA1305
        get => FechaRegistroContable.HasValue ? FechaRegistroContable.Value.ToString("dd/MM/yyyy") : null;
#pragma warning restore CA1305
#pragma warning disable S6580
        set => FechaRegistroContable = string.IsNullOrWhiteSpace(value) ? (DateTime?)null : DateTime.ParseExact(value, "dd/MM/yyyy", null);
#pragma warning restore S6580
    }
    [XmlIgnore] public DateTime? FechaRegistroContable { get; set; }

    [XmlElement("numAutDocSustento", Order = 6)]
    public string NumAutDocSustento { get; set; }

    [XmlElement("pagoLocExt", Order = 7)]
    public string PagoLocExt { get; set; }

    [XmlElement("tipoRegi", Order = 8)]
    public string TipoRegi { get; set; }

    [XmlElement("paisEfecPago", Order = 9)]
    public string PaisEfecPago { get; set; }

    [XmlElement("aplicConvDobTrib", Order = 10)]
    public string AplicConvDobTrib { get; set; }

    [XmlElement("pagExtSujRetNorLeg", Order = 11)]
    public string PagExtSujRetNorLeg { get; set; }

    [XmlElement("pagoRegFis", Order = 12)]
    public string PagoRegFis { get; set; }

    [XmlElement("totalSinImpuestos", Order = 13)]
    public decimal TotalSinImpuestos { get; set; }

    [XmlElement("importeTotal", Order = 14)]
    public decimal ImporteTotal { get; set; }

    [XmlArray("impuestosDocSustento", Order = 15)]
    [XmlArrayItem("impuestoDocSustento")]
    public List<ImpuestoDocSustentoXml>? ImpuestosDocSustento { get; set; } = new();

    [XmlArray("retenciones", Order = 16)]
    [XmlArrayItem("retencion")]
    public List<RetencionXml>? Retenciones { get; set; } = new();

    [XmlElement("reembolsos", Order = 17)]
    public ReembolsoDetalleXml? ReembolsoDetalle { get; set; }

    [XmlArray("pagos", Order = 18)]
    [XmlArrayItem("pago")]
    public List<PagoXml>? Pagos { get; set; } = new();
}

public class ImpuestoDocSustentoXml
{
    [XmlElement("codImpuestoDocSustento", Order = 1)]
    public int CodImpuestoDocSustento { get; set; }

    [XmlElement("codigoPorcentaje", Order = 2)]
    public int CodigoPorcentaje { get; set; }

    [XmlElement("baseImponible", Order = 3)]
    public decimal BaseImponible { get; set; }

    [XmlElement("tarifa", Order = 4)]
    public decimal Tarifa { get; set; }

    [XmlElement("valorImpuesto", Order = 5)]
    public decimal ValorImpuesto { get; set; }
}

public class RetencionXml
{
    [XmlElement("codigo", Order = 1)]
    public int Codigo { get; set; }

    [XmlElement("codigoRetencion", Order = 2)]
    public string CodigoRetencion { get; set; }

    [XmlElement("baseImponible", Order = 3)]
    public decimal BaseImponible { get; set; }

    [XmlElement("porcentajeRetener", Order = 4)]
    public decimal PorcentajeRetener { get; set; }

    [XmlElement("valorRetenido", Order = 5)]
    public decimal ValorRetenido { get; set; }

    // compraCajBanano / dividendos nodes omitted for brevity (add when needed)
}

public class ReembolsoDetalleXml
{
    [XmlElement("tipoIdentificacionProveedorReembolso", Order = 1)]
    public string TipoIdentificacionProveedorReembolso { get; set; }

    [XmlElement("identificacionProveedorReembolso", Order = 2)]
    public string IdentificacionProveedorReembolso { get; set; }

    [XmlElement("codPaisPagoProveedorReembolso", Order = 3)]
    public string CodPaisPagoProveedorReembolso { get; set; }

    [XmlElement("tipoProveedorReembolso", Order = 4)]
    public string TipoProveedorReembolso { get; set; }

    [XmlElement("codDocReembolso", Order = 5)]
    public string CodDocReembolso { get; set; }

    [XmlElement("estabDocReembolso", Order = 6)]
    public string EstabDocReembolso { get; set; }

    [XmlElement("ptoEmiDocReembolso", Order = 7)]
    public string PtoEmiDocReembolso { get; set; }

    [XmlElement("secuencialDocReembolso", Order = 8)]
    public string SecuencialDocReembolso { get; set; }

    [XmlElement("fechaEmisionDocReembolso", Order = 9)]
    public string FechaEmisionDocReembolso { get; set; }

    [XmlElement("numeroAutorizacionDocReemb", Order = 10)]
    public string NumeroAutorizacionDocReemb { get; set; }

    [XmlArray("detalleImpuestos", Order = 11)]
    [XmlArrayItem("detalleImpuesto")]
    public List<DetalleImpuestoReembolsoXml>? DetalleImpuestos { get; set; } = new();
}

public class DetalleImpuestoReembolsoXml
{
    [XmlElement("codigo", Order = 1)]
    public int Codigo { get; set; }

    [XmlElement("codigoPorcentaje", Order = 2)]
    public int CodigoPorcentaje { get; set; }

    [XmlElement("tarifa", Order = 3)]
    public decimal Tarifa { get; set; }

    [XmlElement("baseImponibleReembolso", Order = 4)]
    public decimal BaseImponibleReembolso { get; set; }

    [XmlElement("impuestoReembolso", Order = 5)]
    public decimal ImpuestoReembolso { get; set; }
}

public class PagoXml
{
    [XmlElement("formaPago", Order = 1)]
    public string FormaPago { get; set; }

    [XmlElement("total", Order = 2)]
    public decimal Total { get; set; }
}
