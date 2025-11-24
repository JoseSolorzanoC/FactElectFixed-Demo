using FactElectFixed.Api.Features.NotaCredito.Requests;
using FastEndpoints;
using Infoware.SRI.Core.Enumerados;

namespace FactElectFixed.Api.Requests;

public class FirmarDocumentoRequest<T>
{
    public EnumTipoAmbiente Ambiente { get; set; }
    public string Version { get; set; }
    [FromBody] public List<T> Comprobantes { get; set; }
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
