using FactElectFixed.Api.Features.NotaDebito.Xml;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Helpers.Models;
using FactElectFixed.Api.Requests;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;

namespace FactElectFixed.Api.Features.NotaDebito.Requests;

public class NotaDebitoRequest : IDocumentoElectronicoNoDetalles<NotaDebitoXmlModel>
{
    public InfoNotaDebitoRequest InfoNotaDebitoRequest { get; set; }
    public List<MotivoRequest> Motivos { get; set; }
    public List<CampoAdicionalRequest> InfoAdicional { get; set; }
    public InfoTributariaRequest InfoTributariaRequest { get; set; }

    public NotaDebitoXmlModel ToXml(EnumTipoAmbiente tipoAmbiente, string version)
    {
        return new NotaDebitoXmlModel
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
                ClaveAcceso = Utils.GenerarClaveAcceso(
                    EnumTipoDocumento.NotaDebito,
                    InfoNotaDebitoRequest.FechaEmision,
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
                            Emisor = new Emisor
                            {
                                RUC = InfoTributariaRequest.Ruc,
                                EnumTipoAmbiente = tipoAmbiente
                            }
                        }
                    },
#pragma warning disable CA1305
                    long.Parse(InfoTributariaRequest.Secuencial),
#pragma warning restore CA1305
                    EnumTipoEmision.Normal
                ),

                CodDoc = InfoTributariaRequest.CodDoc,
                Estab = InfoTributariaRequest.Estab,
                PtoEmi = InfoTributariaRequest.PtoEmi,
                Secuencial = InfoTributariaRequest.Secuencial,
                DirMatriz = InfoTributariaRequest.DirMatriz
            },
            InfoNotaDebitoXml = new InfoNotaDebitoXml
            {
                FechaEmision = InfoNotaDebitoRequest.FechaEmision,
                DirEstablecimiento = InfoNotaDebitoRequest.DirEstablecimiento,
                TipoIdentificacionComprador = InfoNotaDebitoRequest.TipoIdentificacionComprador,
                RazonSocialComprador = InfoNotaDebitoRequest.RazonSocialComprador,
                IdentificacionComprador = InfoNotaDebitoRequest.IdentificacionComprador,
                ContribuyenteEspecial = InfoNotaDebitoRequest.ContribuyenteEspecial,
                ObligadoContabilidad = InfoNotaDebitoRequest.ObligadoContabilidad,
                CodDocModificado = InfoNotaDebitoRequest.CodDocModificado,
                NumDocModificado = InfoNotaDebitoRequest.NumDocModificado,
                FechaEmisionDocSustento = InfoNotaDebitoRequest.FechaEmisionDocSustento,
                TotalSinImpuestos = InfoNotaDebitoRequest.TotalSinImpuestos,
                ValorTotal = InfoNotaDebitoRequest.ValorTotal,

                Impuestos = InfoNotaDebitoRequest.Impuestos?.Select(i => new ImpuestoXml
                {
                    Codigo = i.Codigo,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    Tarifa = i.Tarifa,
                    BaseImponible = i.BaseImponible,
                    Valor = i.Valor
                }).ToList(),
                Pagos = InfoNotaDebitoRequest.Pagos?.Select(p => new PagoXml
                {
                    FormaPago = p.FormaPago,
                    Total = p.Total,
                    Plazo = p.Plazo,
                    UnidadTiempo = p.UnidadTiempo
                }).ToList()
            },
            Motivos = Motivos?.Select(m => new MotivoRequest
            {
                Razon = m.Razon,
                Valor = m.Valor
            }).ToList(),
            InfoAdicional = InfoAdicional?.Select(a => new CampoAdicionalXml
            {
                Nombre = a.Nombre,
                Valor = a.Valor
            }).ToList()
        };
    }
}

public class InfoNotaDebitoRequest
{
    public DateTime FechaEmision { get; set; }
    public string DirEstablecimiento { get; set; }
    public string TipoIdentificacionComprador { get; set; }
    public string RazonSocialComprador { get; set; }
    public string IdentificacionComprador { get; set; }
    public string ContribuyenteEspecial { get; set; }
    public bool ObligadoContabilidad { get; set; }
    public string CodDocModificado { get; set; }
    public string NumDocModificado { get; set; }
    public string FechaEmisionDocSustento { get; set; }
    public decimal TotalSinImpuestos { get; set; }
    public List<ImpuestoRequest> Impuestos { get; set; }
    public decimal ValorTotal { get; set; }
    public List<PagoRequest> Pagos { get; set; }
}

public class MotivoRequest
{
    public string Razon { get; set; }
    public decimal Valor { get; set; }
}
