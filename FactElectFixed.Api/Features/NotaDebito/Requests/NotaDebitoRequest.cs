using FactElectFixed.Api.Features.NotaDebito.Xml;
using FactElectFixed.Api.Helpers.Interfaces;
using FactElectFixed.Api.Helpers.Models;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;

namespace FactElectFixed.Api.Features.NotaDebito.Requests;

public class NotaDebitoRequest : IDocumentoElectronicoNoDetalles<NotaDebitoXmlModel>
{
    public InfoNotaDebito InfoNotaDebito { get; set; }
    public List<Motivo> Motivos { get; set; }
    public List<CampoAdicional> InfoAdicional { get; set; }
    public Api.Requests.InfoTributaria InfoTributaria { get; set; }
    public NotaDebitoXmlModel ToXml(EnumTipoAmbiente tipoAmbiente, string version)
    {
        return new NotaDebitoXmlModel
        {
            Version = version,

            InfoTributaria = new InfoTributaria
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

                ClaveAcceso = Utils.GenerarClaveAcceso(
                    EnumTipoDocumento.NotaDebito,
                    InfoNotaDebito.FechaEmision,
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
                            Emisor = new Emisor
                            {
                                RUC = InfoTributaria.Ruc,
                                EnumTipoAmbiente = tipoAmbiente
                            }
                        }
                    },
#pragma warning disable CA1305
                    long.Parse(InfoTributaria.Secuencial),
#pragma warning restore CA1305
                    EnumTipoEmision.Normal
                ),

                CodDoc = InfoTributaria.CodDoc,
                Estab = InfoTributaria.Estab,
                PtoEmi = InfoTributaria.PtoEmi,
                Secuencial = InfoTributaria.Secuencial,
                DirMatriz = InfoTributaria.DirMatriz
            },

            InfoNotaDebito = new Xml.InfoNotaDebito
            {
                FechaEmision = InfoNotaDebito.FechaEmision,
                DirEstablecimiento = InfoNotaDebito.DirEstablecimiento,
                TipoIdentificacionComprador = InfoNotaDebito.TipoIdentificacionComprador,
                RazonSocialComprador = InfoNotaDebito.RazonSocialComprador,
                IdentificacionComprador = InfoNotaDebito.IdentificacionComprador,
                ContribuyenteEspecial = InfoNotaDebito.ContribuyenteEspecial,
                ObligadoContabilidad = InfoNotaDebito.ObligadoContabilidad,
                CodDocModificado = InfoNotaDebito.CodDocModificado,
                NumDocModificado = InfoNotaDebito.NumDocModificado,
                FechaEmisionDocSustento = InfoNotaDebito.FechaEmisionDocSustento,
                TotalSinImpuestos = InfoNotaDebito.TotalSinImpuestos,
                ValorTotal = InfoNotaDebito.ValorTotal,

                Impuestos = InfoNotaDebito.Impuestos?.Select(i => new Impuesto
                {
                    Codigo = i.Codigo,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    Tarifa = i.Tarifa,
                    BaseImponible = i.BaseImponible,
                    Valor = i.Valor
                }).ToList(),

                Pagos = InfoNotaDebito.Pagos?.Select(p => new Pago
                {
                    FormaPago = p.FormaPago,
                    Total = p.Total,
                    Plazo = p.Plazo,
                    UnidadTiempo = p.UnidadTiempo
                }).ToList()
            },

            Motivos = Motivos?.Select(m => new Motivo
            {
                Razon = m.Razon,
                Valor = m.Valor
            }).ToList(),

            InfoAdicional = InfoAdicional?.Select(a => new CampoAdicional
            {
                Nombre = a.Nombre,
                Valor = a.Valor
            }).ToList()
        };
    }
}

public class InfoNotaDebito
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
    public List<Api.Requests.Impuesto> Impuestos { get; set; }
    public decimal ValorTotal { get; set; }
    public List<Api.Requests.Pago> Pagos { get; set; }
}

public class Motivo
{
    public string Razon { get; set; }
    public decimal Valor { get; set; }
}

public class CampoAdicional
{
    public string Nombre { get; set; }
    public string Valor { get; set; }
}
