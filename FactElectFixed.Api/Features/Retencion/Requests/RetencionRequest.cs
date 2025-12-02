using FactElectFixed.Api.Features.Retencion.Xml;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Helpers.Models;
using FactElectFixed.Api.Requests;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;
using PagoXml = FactElectFixed.Api.Features.Retencion.Xml.PagoXml;

namespace FactElectFixed.Api.Features.Retencion.Requests;

public class RetencionRequest : IDocumentoElectronicoNoDetalles<RetencionXmlModel>
{
    public InfoCompRetencionRequest InfoCompRetencionRequest { get; set; }
    public List<DocSustentoRequest> DocsSustento { get; set; } = new();
    public List<CampoAdicionalRequest> InfoAdicional { get; set; } = new();

    public RetencionXmlModel ToXml(EnumTipoAmbiente tipoAmbiente, string version)
    {
        return new RetencionXmlModel
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
                ClaveAcceso = Utils.GenerarClaveAcceso(EnumTipoDocumento.ComprobanteRetencion,
                    InfoCompRetencionRequest.FechaEmision,
                    new PuntoEmision
                    {
#pragma warning disable CA1305
                        Codigo = int.Parse(InfoTributariaRequest.PtoEmi),
#pragma warning restore CA1305
                        Establecimiento = new Establecimiento
                        {
#pragma warning disable CA1305
                            Codigo = int.Parse(InfoTributariaRequest.Estab),
#pragma warning restore CA1305
                            Emisor = new Emisor { RUC = InfoTributariaRequest.Ruc, EnumTipoAmbiente = tipoAmbiente }
                        }
                    },
#pragma warning disable CA1305
                    long.Parse(InfoTributariaRequest.Secuencial),
#pragma warning restore CA1305
                    EnumTipoEmision.Normal),
                CodDoc = InfoTributariaRequest.CodDoc,
                Estab = InfoTributariaRequest.Estab,
                PtoEmi = InfoTributariaRequest.PtoEmi,
                Secuencial = InfoTributariaRequest.Secuencial,
                DirMatriz = InfoTributariaRequest.DirMatriz
            },

            InfoCompRetencion = new InfoCompRetencionXml
            {
                FechaEmision = InfoCompRetencionRequest.FechaEmision,
                DirEstablecimiento = InfoCompRetencionRequest.DirEstablecimiento,
                ContribuyenteEspecial = InfoCompRetencionRequest.ContribuyenteEspecial,
                ObligadoContabilidad = InfoCompRetencionRequest.ObligadoContabilidad ?? false,
                TipoIdentificacionSujetoRetenido = InfoCompRetencionRequest.TipoIdentificacionSujetoRetenido,
                TipoSujetoRetenido = InfoCompRetencionRequest.TipoSujetoRetenido,
                ParteRel = InfoCompRetencionRequest.ParteRel,
                RazonSocialSujetoRetenido = InfoCompRetencionRequest.RazonSocialSujetoRetenido,
                IdentificacionSujetoRetenido = InfoCompRetencionRequest.IdentificacionSujetoRetenido,
                PeriodoFiscal = InfoCompRetencionRequest.PeriodoFiscal
            },

            DocsSustento = DocsSustento?.Select(d => new DocSustentoXml
            {
                CodSustento = d.CodSustento,
                CodDocSustento = d.CodDocSustento,
                NumDocSustento = d.NumDocSustento,
                FechaEmisionDocSustento = d.FechaEmisionDocSustento,
                FechaRegistroContable = d.FechaRegistroContable,
                NumAutDocSustento = d.NumAutDocSustento,
                PagoLocExt = d.PagoLocExt,
                TipoRegi = d.TipoRegi,
                PaisEfecPago = d.PaisEfecPago,
                AplicConvDobTrib = d.AplicConvDobTrib,
                PagExtSujRetNorLeg = d.PagExtSujRetNorLeg,
                PagoRegFis = d.PagoRegFis,
                TotalSinImpuestos = d.TotalSinImpuestos,
                ImporteTotal = d.ImporteTotal,
                ImpuestosDocSustento = d.ImpuestosDocSustento?.Select(i => new ImpuestoDocSustentoXml
                {
                    CodImpuestoDocSustento = i.CodImpuestoDocSustento,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    BaseImponible = i.BaseImponible,
                    Tarifa = i.Tarifa,
                    ValorImpuesto = i.ValorImpuesto
                }).ToList(),
                Retenciones = d.Retenciones?.Select(r => new RetencionXml
                {
                    Codigo = r.Codigo,
                    CodigoRetencion = r.CodigoRetencion,
                    BaseImponible = r.BaseImponible,
                    PorcentajeRetener = r.PorcentajeRetener,
                    ValorRetenido = r.ValorRetenido
                }).ToList(),
                ReembolsoDetalle = d.ReembolsoDetalleRequest == null
                    ? null
                    : new ReembolsoDetalleXml
                    {
                        TipoIdentificacionProveedorReembolso =
                            d.ReembolsoDetalleRequest.TipoIdentificacionProveedorReembolso,
                        IdentificacionProveedorReembolso = d.ReembolsoDetalleRequest.IdentificacionProveedorReembolso,
                        CodPaisPagoProveedorReembolso = d.ReembolsoDetalleRequest.CodPaisPagoProveedorReembolso,
                        TipoProveedorReembolso = d.ReembolsoDetalleRequest.TipoProveedorReembolso,
                        CodDocReembolso = d.ReembolsoDetalleRequest.CodDocReembolso,
                        EstabDocReembolso = d.ReembolsoDetalleRequest.EstabDocReembolso,
                        PtoEmiDocReembolso = d.ReembolsoDetalleRequest.PtoEmiDocReembolso,
                        SecuencialDocReembolso = d.ReembolsoDetalleRequest.SecuencialDocReembolso,
                        FechaEmisionDocReembolso = d.ReembolsoDetalleRequest.FechaEmisionDocReembolso,
                        NumeroAutorizacionDocReemb = d.ReembolsoDetalleRequest.NumeroAutorizacionDocReemb,
                        DetalleImpuestos = d.ReembolsoDetalleRequest.DetalleImpuestos?.Select(t =>
                            new DetalleImpuestoReembolsoXml
                            {
                                Codigo = t.Codigo,
                                CodigoPorcentaje = t.CodigoPorcentaje,
                                Tarifa = t.Tarifa,
                                BaseImponibleReembolso = t.BaseImponibleReembolso,
                                ImpuestoReembolso = t.ImpuestoReembolso
                            }).ToList()
                    },
                Pagos = d.Pagos?.Select(p => new PagoXml
                {
                    FormaPago = p.FormaPago,
                    Total = p.Total
                }).ToList()
            }).ToList(),
            InfoAdicional = InfoAdicional.Select(a => new CampoAdicionalXml
            {
                Nombre = a.Nombre,
                Valor = a.Valor
            }).ToList()
        };
    }

    public InfoTributariaRequest InfoTributariaRequest { get; set; }
}

public class InfoCompRetencionRequest
{
    public DateTime FechaEmision { get; set; }
    public string DirEstablecimiento { get; set; }
    public string ContribuyenteEspecial { get; set; }
    public bool? ObligadoContabilidad { get; set; }
    public string TipoIdentificacionSujetoRetenido { get; set; }
    public string TipoSujetoRetenido { get; set; }
    public string ParteRel { get; set; }
    public string RazonSocialSujetoRetenido { get; set; }
    public string IdentificacionSujetoRetenido { get; set; }
    public string PeriodoFiscal { get; set; }
}

public class DocSustentoRequest
{
    public string CodSustento { get; set; }
    public string CodDocSustento { get; set; }
    public string NumDocSustento { get; set; }
    public DateTime FechaEmisionDocSustento { get; set; }
    public DateTime? FechaRegistroContable { get; set; }
    public string NumAutDocSustento { get; set; }
    public string PagoLocExt { get; set; }
    public string TipoRegi { get; set; }
    public string PaisEfecPago { get; set; }
    public string AplicConvDobTrib { get; set; }
    public string PagExtSujRetNorLeg { get; set; }
    public string PagoRegFis { get; set; }

    public decimal TotalSinImpuestos { get; set; }
    public decimal ImporteTotal { get; set; }

    public List<ImpuestoDocSustentoRequest> ImpuestosDocSustento { get; set; } = new();
    public List<RetencionDetalleRequest> Retenciones { get; set; } = new();
    public ReembolsoDetalleRequest ReembolsoDetalleRequest { get; set; }
    public List<PagoRequest> Pagos { get; set; } = new();
}

public class ImpuestoDocSustentoRequest
{
    public int CodImpuestoDocSustento { get; set; }
    public int CodigoPorcentaje { get; set; }
    public decimal BaseImponible { get; set; }
    public decimal Tarifa { get; set; }
    public decimal ValorImpuesto { get; set; }
}

public class RetencionDetalleRequest
{
    public int Codigo { get; set; }
    public string CodigoRetencion { get; set; }
    public decimal BaseImponible { get; set; }
    public decimal PorcentajeRetener { get; set; }
    public decimal ValorRetenido { get; set; }
}

public class ReembolsoDetalleRequest
{
    public string TipoIdentificacionProveedorReembolso { get; set; }
    public string IdentificacionProveedorReembolso { get; set; }
    public string CodPaisPagoProveedorReembolso { get; set; }
    public string TipoProveedorReembolso { get; set; }
    public string CodDocReembolso { get; set; }
    public string EstabDocReembolso { get; set; }
    public string PtoEmiDocReembolso { get; set; }
    public string SecuencialDocReembolso { get; set; }
    public string FechaEmisionDocReembolso { get; set; }
    public string NumeroAutorizacionDocReemb { get; set; }

    public List<DetalleImpuestoReembolsoRequest> DetalleImpuestos { get; set; } = new();
}

public class DetalleImpuestoReembolsoRequest
{
    public int Codigo { get; set; }
    public int CodigoPorcentaje { get; set; }
    public decimal Tarifa { get; set; }
    public decimal BaseImponibleReembolso { get; set; }
    public decimal ImpuestoReembolso { get; set; }
}
