using System.ComponentModel.DataAnnotations;
using FastEndpoints;

namespace FactElectFixed.Api.Features.Factura.Firma.Requests;

public class FirmarFacturaRequest
{
    public string Ambiente { get; set; }
    public string Version { get; set; }
    [FromBody]
    public Factura FacturaData { get; set; }
}

public class Factura
{
    [Required]
    public InfoTributaria InfoTributaria { get; set; }

    [Required]
    public InfoFactura InfoFactura { get; set; }

    [Required]
    public List<Detalle> Detalles { get; set; }

    public List<Rubro> OtrosRubrosTerceros { get; set; }
}
public class InfoTributaria
{
    [Required]
    [Range(1, 2)]
    public int Ambiente { get; set; }

    [Required]
    [Range(1, 1)]
    public int TipoEmision { get; set; }

    [Required]
    [StringLength(300)]
    public string RazonSocial { get; set; }

    [StringLength(300)]
    public string NombreComercial { get; set; }

    [Required]
    [StringLength(13, MinimumLength = 13)]
    public string Ruc { get; set; }
    
    [Required]
    [StringLength(2)]
    public string CodDoc { get; set; }

    [Required]
    [StringLength(3)]
    public string Estab { get; set; }

    [Required]
    [StringLength(3)]
    public string PtoEmi { get; set; }

    [Required]
    [StringLength(9)]
    public string Secuencial { get; set; }

    [Required]
    [StringLength(300)]
    public string DirMatriz { get; set; }
}

public class InfoFactura
{
    [Required]
    public DateTime FechaEmision { get; set; }

    [StringLength(300)]
    public string DirEstablecimiento { get; set; }

    [StringLength(13, MinimumLength = 3)]
    public string ContribuyenteEspecial { get; set; }

    public bool ObligadoContabilidad { get; set; } // SI/NO

    [Required]
    [StringLength(2)]
    public string TipoIdentificacionComprador { get; set; }

    [Required]
    [StringLength(300)]
    public string RazonSocialComprador { get; set; }

    [Required]
    [StringLength(13)]
    public string IdentificacionComprador { get; set; }

    [StringLength(300)]
    public string DireccionComprador { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal TotalSinImpuestos { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal TotalDescuento { get; set; }

    [Required]
    public List<TotalImpuesto> TotalConImpuestos { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal Propina { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal ImporteTotal { get; set; }

    [StringLength(15)]
    public string Moneda { get; set; }

    [Required]
    public List<Pago> Pagos { get; set; }

    [Range(0, 999999999999.99)]
    public decimal? ValorRetIva { get; set; }

    [Range(0, 999999999999.99)]
    public decimal? ValorRetRenta { get; set; }
}

public class TotalImpuesto
{
    [Required]
    [AllowedValues(2, 3, 5)]
    public int Codigo { get; set; }

    [Required]
    [Range(1, 9999)]
    public int CodigoPorcentaje { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal BaseImponible { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal Valor { get; set; }
}

public class Pago
{
    [Required]
    [StringLength(2)]
    public string FormaPago { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal Total { get; set; }

    [Range(0, 999999999999.99)]
    public decimal Plazo { get; set; }

    [StringLength(10)]
    public string UnidadTiempo { get; set; }
}

public class Detalle
{
    [Required]
    [StringLength(25)]
    public string CodigoPrincipal { get; set; }

    [StringLength(25)]
    public string CodigoAuxiliar { get; set; }

    [Required]
    [StringLength(300)]
    public string Descripcion { get; set; }

    [Required]
    [Range(0, 999999999999.999999)]
    public decimal Cantidad { get; set; }

    [Required]
    [Range(0, 999999999999.999999)]
    public decimal PrecioUnitario { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal Descuento { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal PrecioTotalSinImpuesto { get; set; }

    [Required]
    public List<Impuesto> Impuestos { get; set; }
}

public class Impuesto
{
    [Required]
    [AllowedValues(2, 3, 5)]
    public int Codigo { get; set; }

    [Required]
    [Range(1, 9999)]
    public int CodigoPorcentaje { get; set; }

    [Required]
    [Range(0.01, 9999.99)]
    public decimal Tarifa { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal BaseImponible { get; set; }

    [Required]
    [Range(0, 999999999999.99)]
    public decimal Valor { get; set; }
}

public class Rubro
{
    [Required] [StringLength(300)] public string Concepto { get; set; }

    [Required] [Range(1, 9999)] public decimal Total { get; set; }
}
