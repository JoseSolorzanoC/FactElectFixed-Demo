using FactElectFixed.Api.Features.NotaDebito.Requests;
using FactElectFixed.Api.Features.Retencion.Xml;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Requests;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;
using InfoTributaria = FactElectFixed.Api.Requests.InfoTributaria;

namespace FactElectFixed.Api.Features.Retencion.Requests;

public class RetencionRequest : IDocumentoElectronicoNoDetalles<RetencionXmlModel>
{
    public RetencionXmlModel ToXml(EnumTipoAmbiente tipoAmbiente, string version)
    {
return new RetencionXmlModel
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
            ClaveAcceso = Utils.GenerarClaveAcceso(EnumTipoDocumento.ComprobanteRetencion,
                InfoCompRetencion.FechaEmision,
                new PuntoEmision
                {
#pragma warning disable CA1305
                    Codigo = int.Parse(InfoTributaria.PtoEmi),
#pragma warning restore CA1305
                    Establecimiento = new Establecimiento
                    {
#pragma warning disable CA1305
                        Codigo = int.Parse(InfoTributaria.Estab),
#pragma warning restore CA1305
                        Emisor = new Emisor { RUC = InfoTributaria.Ruc, EnumTipoAmbiente = tipoAmbiente }
                    }
                },
#pragma warning disable CA1305
                long.Parse(InfoTributaria.Secuencial),
#pragma warning restore CA1305
                EnumTipoEmision.Normal),
            CodDoc = InfoTributaria.CodDoc,
            Estab = InfoTributaria.Estab,
            PtoEmi = InfoTributaria.PtoEmi,
            Secuencial = InfoTributaria.Secuencial,
            DirMatriz = InfoTributaria.DirMatriz
        },

        InfoCompRetencion = new InfoCompRetencionXml
        {
            FechaEmision = InfoCompRetencion.FechaEmision,
            DirEstablecimiento = InfoCompRetencion.DirEstablecimiento,
            ContribuyenteEspecial = InfoCompRetencion.ContribuyenteEspecial,
            ObligadoContabilidad = InfoCompRetencion.ObligadoContabilidad ?? false,
            TipoIdentificacionSujetoRetenido = InfoCompRetencion.TipoIdentificacionSujetoRetenido,
            TipoSujetoRetenido = InfoCompRetencion.TipoSujetoRetenido,
            ParteRel = InfoCompRetencion.ParteRel,
            RazonSocialSujetoRetenido = InfoCompRetencion.RazonSocialSujetoRetenido,
            IdentificacionSujetoRetenido = InfoCompRetencion.IdentificacionSujetoRetenido,
            PeriodoFiscal = InfoCompRetencion.PeriodoFiscal
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
            ReembolsoDetalle = d.ReembolsoDetalle == null ? null : new ReembolsoDetalleXml
            {
                TipoIdentificacionProveedorReembolso = d.ReembolsoDetalle.TipoIdentificacionProveedorReembolso,
                IdentificacionProveedorReembolso = d.ReembolsoDetalle.IdentificacionProveedorReembolso,
                CodPaisPagoProveedorReembolso = d.ReembolsoDetalle.CodPaisPagoProveedorReembolso,
                TipoProveedorReembolso = d.ReembolsoDetalle.TipoProveedorReembolso,
                CodDocReembolso = d.ReembolsoDetalle.CodDocReembolso,
                EstabDocReembolso = d.ReembolsoDetalle.EstabDocReembolso,
                PtoEmiDocReembolso = d.ReembolsoDetalle.PtoEmiDocReembolso,
                SecuencialDocReembolso = d.ReembolsoDetalle.SecuencialDocReembolso,
                FechaEmisionDocReembolso = d.ReembolsoDetalle.FechaEmisionDocReembolso,
                NumeroAutorizacionDocReemb = d.ReembolsoDetalle.NumeroAutorizacionDocReemb,
                DetalleImpuestos = d.ReembolsoDetalle.DetalleImpuestos?.Select(t => new DetalleImpuestoReembolsoXml
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

        InfoAdicional = InfoAdicional?.Select(a => new CampoAdicionalXml
        {
            Nombre = a.Nombre,
            Valor = a.Valor
        }).ToList()
    };
    }

    public InfoTributaria InfoTributaria { get; set; }
    public InfoCompRetencion InfoCompRetencion { get; set; }
    public List<DocSustento> DocsSustento { get; set; } = new();
    public List<CampoAdicional> InfoAdicional { get; set; } = new();
}

public class InfoCompRetencion
{
    public DateTime FechaEmision { get; set; }
    public string DirEstablecimiento { get; set; }
    public string ContribuyenteEspecial { get; set; }
    public bool? ObligadoContabilidad { get; set; }
    public string TipoIdentificacionSujetoRetenido { get; set; }
    public string TipoSujetoRetenido { get; set; }
    public string ParteRel { get; set; } // "SI"/"NO"
    public string RazonSocialSujetoRetenido { get; set; }
    public string IdentificacionSujetoRetenido { get; set; }
    public string PeriodoFiscal { get; set; } // mm/yyyy
}

public class DocSustento
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

    public List<ImpuestoDocSustento> ImpuestosDocSustento { get; set; } = new();
    public List<RetencionDetalle> Retenciones { get; set; } = new();
    public ReembolsoDetalle ReembolsoDetalle { get; set; } // opcional
    public List<Pago> Pagos { get; set; } = new();
}

public class ImpuestoDocSustento
{
    public int CodImpuestoDocSustento { get; set; } // codImpuestoDocSustento
    public int CodigoPorcentaje { get; set; }
    public decimal BaseImponible { get; set; }
    public decimal Tarifa { get; set; }
    public decimal ValorImpuesto { get; set; }
}

public class RetencionDetalle
{
    public int Codigo { get; set; }
    public string CodigoRetencion { get; set; }
    public decimal BaseImponible { get; set; }
    public decimal PorcentajeRetener { get; set; }
    public decimal ValorRetenido { get; set; }
}

public class ReembolsoDetalle
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

    public List<DetalleImpuestoReembolso> DetalleImpuestos { get; set; } = new();
}

public class DetalleImpuestoReembolso
{
    public int Codigo { get; set; }
    public int CodigoPorcentaje { get; set; }
    public decimal Tarifa { get; set; }
    public decimal BaseImponibleReembolso { get; set; }
    public decimal ImpuestoReembolso { get; set; }
}

