using FactElectFixed.Api.Features.NotaCredito.Xml;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Requests;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;

namespace FactElectFixed.Api.Features.NotaCredito.Requests;

public class NotaCreditoRequest : IDocumentoElectronico<NotaCreditoXmlModel>
{
   public NotaCreditoXmlModel ToXml(EnumTipoAmbiente tipoAmbiente, string version)
   {
        return new NotaCreditoXmlModel()
        {
            Version = version,
            InfoTributaria = new Api.Helpers.Models.InfoTributaria
            {
#pragma warning disable CA1305
#pragma warning disable S6562
                Ambiente = Convert.ToInt32(tipoAmbiente.ObtenerSRICodigo(validInDate: new DateTime())),
#pragma warning restore S6562
#pragma warning restore CA1305
                TipoEmision = InfoTributaria.TipoEmision,
                RazonSocial = InfoTributaria.RazonSocial,
                NombreComercial = InfoTributaria.NombreComercial,
                Ruc = InfoTributaria.Ruc,
#pragma warning disable CA1305
                ClaveAcceso = Utils.GenerarClaveAcceso(EnumTipoDocumento.Factura, InfoNotaCredito.FechaEmision,
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
            InfoNotaCredito = new Xml.InfoNotaCredito()
            {
                FechaEmision = InfoNotaCredito.FechaEmision,
                DirEstablecimiento = InfoNotaCredito.DirEstablecimiento,
                ContribuyenteEspecial = InfoNotaCredito.ContribuyenteEspecial,
                ObligadoContabilidad = InfoNotaCredito.ObligadoContabilidad,
                TipoIdentificacionComprador = InfoNotaCredito.TipoIdentificacionComprador,
                RazonSocialComprador = InfoNotaCredito.RazonSocialComprador,
                IdentificacionComprador = InfoNotaCredito.IdentificacionComprador,
                TotalSinImpuestos = InfoNotaCredito.TotalSinImpuestos,
                TotalConImpuestos = InfoNotaCredito.TotalConImpuestos.Select(i => new Api.Helpers.Models.TotalImpuesto
                {
                    Codigo = i.Codigo,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    BaseImponible = i.BaseImponible,
                    Valor = i.Valor
                }).ToList(),
                Moneda = InfoNotaCredito.Moneda,
            },
            Detalles = Detalles.Select(d => new Detalle
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

   public InfoNotaCredito InfoNotaCredito { get; set; }
   public Api.Requests.InfoTributaria InfoTributaria { get; set; }
   public List<Api.Requests.Detalle> Detalles { get; set; }
}

public class InfoNotaCredito
{
    public DateTime FechaEmision { get; set; }
    public string DirEstablecimiento { get; set; }
    public string TipoIdentificacionComprador { get; set; }
    public string RazonSocialComprador { get; set; }
    public string IdentificacionComprador { get; set; }
    public string ContribuyenteEspecial { get; set; }
    public bool ObligadoContabilidad { get; set; }
    public string Rise { get; set; }
    public string CodDocModificado { get; set; }
    public string NumDocModificado { get; set; }
    public string FechaEmisionDocSustento { get; set; }
    public decimal TotalSinImpuestos { get; set; }
    public decimal ValorModificacion { get; set; }
    public string Moneda { get; set; }
    public List<TotalImpuesto> TotalConImpuestos { get; set; }
    public string Motivo { get; set; } 
}
