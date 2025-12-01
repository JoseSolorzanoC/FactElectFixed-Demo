using FactElectFixed.Api.Features.Factura.Xml;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Helpers.Models;
using FactElectFixed.Api.Requests;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;

namespace FactElectFixed.Api.Features.Factura.Requests;

public class FacturaRequest : IDocumentoElectronico<FacturaXmlModel>
{
    public InfoFactura InfoFactura { get; set; }
    public InfoTributariaRequest InfoTributariaRequest { get; set; }
    public List<DetalleRequest> Detalles { get; set; }

    public FacturaXmlModel ToXml(EnumTipoAmbiente tipoAmbiente, string version)
    {
        return new FacturaXmlModel
        {
            Version = version,
            InfoTributariaXml = new Helpers.Models.InfoTributariaXml
            {
#pragma warning disable CA1305
#pragma warning disable S6562
                Ambiente = Convert.ToInt32(tipoAmbiente.ObtenerSRICodigo(new DateTime())),
#pragma warning restore S6562
#pragma warning restore CA1305
                TipoEmision = InfoTributariaRequest.TipoEmision,
                RazonSocial = InfoTributariaRequest.RazonSocial,
                NombreComercial = InfoTributariaRequest.NombreComercial,
                Ruc = InfoTributariaRequest.Ruc,
#pragma warning disable CA1305
                ClaveAcceso = Utils.GenerarClaveAcceso(EnumTipoDocumento.Factura, InfoFactura.FechaEmision,
                    new PuntoEmision
                    {
                        Codigo = int.Parse(InfoTributariaRequest.PtoEmi),
                        Establecimiento = new Establecimiento
                        {
                            Codigo = int.Parse(InfoTributariaRequest.Estab),
                            Emisor = new Emisor
                                { RUC = InfoTributariaRequest.Ruc, EnumTipoAmbiente = tipoAmbiente }
                        }
                    }, long.Parse(InfoTributariaRequest.Secuencial), EnumTipoEmision.Normal),
#pragma warning restore CA1305
                CodDoc = InfoTributariaRequest.CodDoc,
                Estab = InfoTributariaRequest.Estab,
                PtoEmi = InfoTributariaRequest.PtoEmi,
                Secuencial = InfoTributariaRequest.Secuencial,
                DirMatriz = InfoTributariaRequest.DirMatriz
            },
            InfoFacturaXml = new Xml.InfoFacturaXml
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
                TotalConImpuestos = InfoFactura.TotalConImpuestos.Select(i => new Helpers.Models.TotalImpuestoXml
                {
                    Codigo = i.Codigo,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    BaseImponible = i.BaseImponible,
                    Valor = i.Valor
                }).ToList(),
                Propina = InfoFactura.Propina,
                ImporteTotal = InfoFactura.ImporteTotal,
                Moneda = InfoFactura.Moneda,
                Pagos = InfoFactura.Pagos.Select(p => new Helpers.Models.PagoXml
                {
                    FormaPago = p.FormaPago,
                    Total = p.Total,
                    Plazo = p.Plazo,
                    UnidadTiempo = p.UnidadTiempo
                }).ToList(),
                ValorRetIva = InfoFactura.ValorRetIva,
                ValorRetRenta = InfoFactura.ValorRetRenta
            },
            Detalles = Detalles.Select(d => new Helpers.Models.DetalleXml
            {
                CodigoPrincipal = d.CodigoPrincipal,
                CodigoAuxiliar = d.CodigoAuxiliar,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Descuento = d.Descuento,
                PrecioTotalSinImpuesto = d.PrecioTotalSinImpuesto,
                Impuestos = d.Impuestos.Select(i => new ImpuestoXml
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
    public List<TotalImpuestoRequest> TotalConImpuestos { get; set; }
    public decimal Propina { get; set; }
    public decimal ImporteTotal { get; set; }
    public string Moneda { get; set; }
    public List<PagoRequest> Pagos { get; set; }
    public decimal? ValorRetIva { get; set; }
    public decimal? ValorRetRenta { get; set; }
}
