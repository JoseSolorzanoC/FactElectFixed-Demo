using FactElectFixed.Api.Features.Factura.Xml;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Requests;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;
using Impuesto = FactElectFixed.Api.Helpers.Models.Impuesto;

namespace FactElectFixed.Api.Features.Factura.Requests;

public class FacturaRequest : IDocumentoElectronico<FacturaXmlModel>
{
    public InfoFactura InfoFactura { get; set; }
    public InfoTributaria InfoTributaria { get; set; }
    public List<Detalle> Detalles { get; set; }

    public FacturaXmlModel ToXml(EnumTipoAmbiente tipoAmbiente, string version)
    {
        return new FacturaXmlModel
        {
            Version = version,
            InfoTributaria = new Helpers.Models.InfoTributaria
            {
#pragma warning disable CA1305
#pragma warning disable S6562
                Ambiente = Convert.ToInt32(tipoAmbiente.ObtenerSRICodigo(new DateTime())),
#pragma warning restore S6562
#pragma warning restore CA1305
                TipoEmision = InfoTributaria.TipoEmision,
                RazonSocial = InfoTributaria.RazonSocial,
                NombreComercial = InfoTributaria.NombreComercial,
                Ruc = InfoTributaria.Ruc,
#pragma warning disable CA1305
                ClaveAcceso = Utils.GenerarClaveAcceso(EnumTipoDocumento.Factura, InfoFactura.FechaEmision,
                    new PuntoEmision
                    {
                        Codigo = int.Parse(InfoTributaria.PtoEmi),
                        Establecimiento = new Establecimiento
                        {
                            Codigo = int.Parse(InfoTributaria.Estab),
                            Emisor = new Emisor
                                { RUC = InfoTributaria.Ruc, EnumTipoAmbiente = tipoAmbiente }
                        }
                    }, long.Parse(InfoTributaria.Secuencial), EnumTipoEmision.Normal),
#pragma warning restore CA1305
                CodDoc = InfoTributaria.CodDoc,
                Estab = InfoTributaria.Estab,
                PtoEmi = InfoTributaria.PtoEmi,
                Secuencial = InfoTributaria.Secuencial,
                DirMatriz = InfoTributaria.DirMatriz
            },
            InfoFactura = new Xml.InfoFactura
            {
                FechaEmision = InfoFactura.FechaEmision,
                DirEstablecimiento = InfoFactura.DirEstablecimiento,
                ContribuyenteEspecial = InfoFactura.ContribuyenteEspecial,
                ObligadoContabilidad = InfoFactura.ObligadoContabilidad,
                TipoIdentificacionComprador = InfoFactura.TipoIdentificacionComprador,
                RazonSocialComprador = InfoFactura.RazonSocialComprador,
                IdentificacionComprador = InfoFactura.IdentificacionComprador,
                DireccionComprador = InfoFactura.DireccionComprador,
                TotalSinImpuestos = InfoFactura.TotalSinImpuestos,
                TotalDescuento = InfoFactura.TotalDescuento,
                TotalConImpuestos = InfoFactura.TotalConImpuestos.Select(i => new Helpers.Models.TotalImpuesto
                {
                    Codigo = i.Codigo,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    BaseImponible = i.BaseImponible,
                    Valor = i.Valor
                }).ToList(),
                Propina = InfoFactura.Propina,
                ImporteTotal = InfoFactura.ImporteTotal,
                Moneda = InfoFactura.Moneda,
                Pagos = InfoFactura.Pagos.Select(p => new Helpers.Models.Pago
                {
                    FormaPago = p.FormaPago,
                    Total = p.Total,
                    Plazo = p.Plazo,
                    UnidadTiempo = p.UnidadTiempo
                }).ToList(),
                ValorRetIva = InfoFactura.ValorRetIva,
                ValorRetRenta = InfoFactura.ValorRetRenta
            },
            Detalles = Detalles.Select(d => new Helpers.Models.Detalle
            {
                CodigoPrincipal = d.CodigoPrincipal,
                CodigoAuxiliar = d.CodigoAuxiliar,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Descuento = d.Descuento,
                PrecioTotalSinImpuesto = d.PrecioTotalSinImpuesto,
                Impuestos = d.Impuestos.Select(i => new Impuesto
                {
                    Codigo = i.Codigo,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    Tarifa = i.Tarifa,
                    BaseImponible = i.BaseImponible,
                    Valor = i.Valor
                }).ToList()
            }).ToList()
        };
    }
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
