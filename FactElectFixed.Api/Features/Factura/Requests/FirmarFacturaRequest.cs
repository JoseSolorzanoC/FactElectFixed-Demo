using FastEndpoints;
using Infoware.SRI.Core.Enumerados;

namespace FactElectFixed.Api.Features.Factura.Requests;

public class FirmarFacturaRequest
{
    public EnumTipoAmbiente Ambiente { get; set; }
    public string Version { get; set; }

    [FromBody] public List<Factura> Comprobantes { get; set; }
}

public class Factura
{
    public InfoTributaria InfoTributaria { get; set; }
    public InfoFactura InfoFactura { get; set; }
    public List<Detalle> Detalles { get; set; }
}

public class InfoTributaria
{
    public int TipoEmision { get; set; }
    public string RazonSocial { get; set; }
    public string NombreComercial { get; set; }
    public string Ruc { get; set; }
    public string CodDoc { get; set; }
    public string Estab { get; set; }
    public string PtoEmi { get; set; }
    public string Secuencial { get; set; }
    public string DirMatriz { get; set; }
}

public class InfoFactura
{
    public DateTime FechaEmision { get; set; }
    public string DirEstablecimiento { get; set; }
    public string ContribuyenteEspecial { get; set; }
    public bool ObligadoContabilidad { get; set; } // SI/NO
    public string TipoIdentificacionComprador { get; set; }
    public string RazonSocialComprador { get; set; }
    public string IdentificacionComprador { get; set; }
    public string DireccionComprador { get; set; }
    public decimal TotalSinImpuestos { get; set; }
    public decimal TotalDescuento { get; set; }
    public List<TotalImpuesto> TotalConImpuestos { get; set; }
    public decimal Propina { get; set; }
    public decimal ImporteTotal { get; set; }
    public string Moneda { get; set; }
    public List<Pago> Pagos { get; set; }
    public decimal? ValorRetIva { get; set; }
    public decimal? ValorRetRenta { get; set; }
}

public class TotalImpuesto
{
    public int Codigo { get; set; }
    public int CodigoPorcentaje { get; set; }
    public decimal BaseImponible { get; set; }
    public decimal Valor { get; set; }
}

public class Pago
{
    public string FormaPago { get; set; }
    public decimal Total { get; set; }
    public decimal Plazo { get; set; }
    public string UnidadTiempo { get; set; }
}

public class Detalle
{
    public string CodigoPrincipal { get; set; }
    public string CodigoAuxiliar { get; set; }
    public string Descripcion { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Descuento { get; set; }
    public decimal PrecioTotalSinImpuesto { get; set; }
    public List<Impuesto> Impuestos { get; set; }
}

public class Impuesto
{
    public int Codigo { get; set; }
    public int CodigoPorcentaje { get; set; }
    public decimal Tarifa { get; set; }
    public decimal BaseImponible { get; set; }
    public decimal Valor { get; set; }
}
