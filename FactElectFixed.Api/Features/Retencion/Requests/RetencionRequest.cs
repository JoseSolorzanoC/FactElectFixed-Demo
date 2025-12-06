using FactElectFixed.Api.Features.Retencion.Xml;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Helpers.Models;
using FactElectFixed.Api.Requests;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;

namespace FactElectFixed.Api.Features.Retencion.Requests;

public class RetencionRequest : IDocumentoElectronicoNoDetalles<RetencionXmlModel>
{
    public InfoTributariaRequest InfoTributariaRequest { get; set; }
    public InfoCompRetencion InfoCompRetencion { get; set; }
    public List<ImpuestoRetencionRequest> Impuestos { get; set; }
    public List<CampoAdicionalRequest> InfoAdicional { get; set; }

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
                    InfoCompRetencion.FechaEmision,
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
                FechaEmision = InfoCompRetencion.FechaEmision,
                DirEstablecimiento = InfoCompRetencion.DirEstablecimiento,
                ContribuyenteEspecial = InfoCompRetencion.ContribuyenteEspecial,
                ObligadoContabilidad = InfoCompRetencion.ObligadoContabilidad ?? false,
                TipoIdentificacionSujetoRetenido = InfoCompRetencion.TipoIdentificacionSujetoRetenido,
                RazonSocialSujetoRetenido = InfoCompRetencion.RazonSocialSujetoRetenido,
                IdentificacionSujetoRetenido = InfoCompRetencion.IdentificacionSujetoRetenido,
                PeriodoFiscal = InfoCompRetencion.PeriodoFiscal
            },
            Impuestos = Impuestos.Select(i => new ImpuestoRetencionXml
            {
                Codigo = i.Codigo,
                CodigoRetencion = i.CodigoRetencion,
                BaseImponible = i.BaseImponible,
                PorcentajeRetener = i.PorcentajeRetener,
                ValorRetenido = i.ValorRetenido,
                CodDocSustento = i.CodDocSustento,
                NumDocSustento = i.NumDocSustento,
                FechaEmisionDocSustento = i.FechaEmisionDocSustento
            }).ToList(),
            InfoAdicional = InfoAdicional.Select(a => new CampoAdicionalXml
            {
                Nombre = a.Nombre,
                Valor = a.Valor
            }).ToList()
        };
    }
}

public class InfoCompRetencion
{
    public DateTime FechaEmision { get; set; }
    public string DirEstablecimiento { get; set; }
    public string ContribuyenteEspecial { get; set; }
    public bool? ObligadoContabilidad { get; set; }
    public string TipoIdentificacionSujetoRetenido { get; set; }
    public string RazonSocialSujetoRetenido { get; set; }
    public string IdentificacionSujetoRetenido { get; set; }
    public string PeriodoFiscal { get; set; }
}

public class ImpuestoRetencionRequest
{
    public int Codigo { get; set; } 
    public string CodigoRetencion { get; set; } 
    public decimal BaseImponible { get; set; }
    public decimal PorcentajeRetener { get; set; }
    public decimal ValorRetenido { get; set; }
    public string CodDocSustento { get; set; } 
    public string NumDocSustento { get; set; } 
    public DateTime FechaEmisionDocSustento { get; set; } 
}
