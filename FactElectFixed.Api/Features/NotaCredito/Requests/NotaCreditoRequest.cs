using FactElectFixed.Api.Features.NotaCredito.Xml;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Helpers.Models;
using FactElectFixed.Api.Requests;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;

namespace FactElectFixed.Api.Features.NotaCredito.Requests;

public class NotaCreditoRequest : IDocumentoElectronicoNoDetalles<NotaCreditoXmlModel>
{
    public InfoNotaCreditoRequest InfoNotaCreditoRequest { get; set; }
    public List<DetalleNotaCreditoRequest> Detalles { get; set; }
    public List<CampoAdicionalRequest> InfoAdicional { get; set; }
    public InfoTributariaRequest InfoTributariaRequest { get; set; }
    public NotaCreditoXmlModel ToXml(EnumTipoAmbiente tipoAmbiente, string version)
    {
        return new NotaCreditoXmlModel
        {
            Version = version,
            InfoTributariaXml = new InfoTributariaXml
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
                ClaveAcceso = Utils.GenerarClaveAcceso(EnumTipoDocumento.NotaCredito,
                    InfoNotaCreditoRequest.FechaEmision,
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
            InfoNotaCreditoXml = new InfoNotaCreditoXml
            {
                FechaEmision = InfoNotaCreditoRequest.FechaEmision,
                FechaEmisionDocSustento = InfoNotaCreditoRequest.FechaEmisionDocSustento,
                Rise = InfoNotaCreditoRequest.Rise,
                CodDocModificado = InfoNotaCreditoRequest.CodDocModificado,
                NumDocModificado = InfoNotaCreditoRequest.NumDocModificado,
                Motivo = InfoNotaCreditoRequest.Motivo,
                ValorModificacion = InfoNotaCreditoRequest.ValorModificacion,
                DirEstablecimiento = InfoNotaCreditoRequest.DirEstablecimiento,
                ContribuyenteEspecial = InfoNotaCreditoRequest.ContribuyenteEspecial,
                ObligadoContabilidad = InfoNotaCreditoRequest.ObligadoContabilidad,
                TipoIdentificacionComprador = InfoNotaCreditoRequest.TipoIdentificacionComprador,
                RazonSocialComprador = InfoNotaCreditoRequest.RazonSocialComprador,
                IdentificacionComprador = InfoNotaCreditoRequest.IdentificacionComprador,
                TotalSinImpuestos = InfoNotaCreditoRequest.TotalSinImpuestos,
                TotalConImpuestos = [.. InfoNotaCreditoRequest.TotalConImpuestos.Select(i => new TotalImpuestoXml
                {
                    Codigo = i.Codigo,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    BaseImponible = i.BaseImponible,
                    Valor = i.Valor
                })],
                Moneda = InfoNotaCreditoRequest.Moneda
            },
            Detalles = [.. Detalles.Select(d => new DetalleNotaCreditoXml
            {
                CodigoInterno = d.CodigoInterno,
                CodigoAdicional = d.CodigoAdicional,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Descuento = d.Descuento ?? 0,
                PrecioTotalSinImpuesto = d.PrecioTotalSinImpuesto,
                DetallesAdicionales = d.DetallesAdicionales?.Select(d => new DetAdicionalXml
                {
                    Nombre = d.Nombre,
                    Valor = d.Valor
                }).ToList(),
                Impuestos = [.. d.Impuestos.Select(i => new ImpuestoXml
                {
                    Codigo = i.Codigo,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    Tarifa = i.Tarifa,
                    BaseImponible = i.BaseImponible,
                    Valor = i.Valor
                })]
            })],
            InfoAdicional = [.. InfoAdicional.Select(i => new CampoAdicionalXml
            {
                Nombre = i.Nombre,
                Valor = i.Valor
            })]
        };
    }
}

public class InfoNotaCreditoRequest
{
    public DateTime FechaEmision { get; set; }
    public string? DirEstablecimiento { get; set; }
    public string TipoIdentificacionComprador { get; set; }
    public string RazonSocialComprador { get; set; }
    public string IdentificacionComprador { get; set; }
    public string? ContribuyenteEspecial { get; set; }
    public bool ObligadoContabilidad { get; set; }
    public string? Rise { get; set; }
    public string CodDocModificado { get; set; }
    public string? NumDocModificado { get; set; }
    public DateTime FechaEmisionDocSustento { get; set; }
    public decimal TotalSinImpuestos { get; set; }
    public decimal ValorModificacion { get; set; }
    public string? Moneda { get; set; }
    public List<TotalImpuestoRequest> TotalConImpuestos { get; set; }
    public string Motivo { get; set; }
}

public class DetalleNotaCreditoRequest
{
    public string? CodigoInterno { get; set; }
    public string? CodigoAdicional { get; set; }
    public string Descripcion { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal? Descuento { get; set; }
    public decimal PrecioTotalSinImpuesto { get; set; }

    public List<DetAdicionalRequest>? DetallesAdicionales { get; set; }
    public List<ImpuestoRequest> Impuestos { get; set; }
}

public class DetAdicionalRequest
{
    public string Nombre { get; set; }
    public string Valor { get; set; }
}
