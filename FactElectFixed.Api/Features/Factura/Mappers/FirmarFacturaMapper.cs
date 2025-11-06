using System.Xml;
using System.Xml.Linq;
using FactElectFixed.Api.Features.Factura.Xml;
using Infoware.Core.Extensions;
using Infoware.SRI.Core.Enumerados;
using Infoware.SRI.Modelos;
using Infoware.SRI.Modelos.Enumerados;
using Infoware.SRI.XSDs;

namespace FactElectFixed.Api.Features.Factura.Mappers;

public static class FirmarFacturaMapper
{
    public static FacturaXmlModel ToXml(this Requests.Factura f, EnumTipoAmbiente tipoAmbiente, string version)
    {
        return new FacturaXmlModel
        {
            Version = version,
            InfoTributaria = new InfoTributaria
            {
#pragma warning disable CA1305
#pragma warning disable S6562
                Ambiente = Convert.ToInt32(tipoAmbiente.ObtenerSRICodigo(validInDate: new DateTime())),
#pragma warning restore S6562
#pragma warning restore CA1305
                TipoEmision = f.InfoTributaria.TipoEmision,
                RazonSocial = f.InfoTributaria.RazonSocial,
                NombreComercial = f.InfoTributaria.NombreComercial,
                Ruc = f.InfoTributaria.Ruc,
#pragma warning disable CA1305
                ClaveAcceso = Utils.GenerarClaveAcceso(EnumTipoDocumento.Factura, f.InfoFactura.FechaEmision,
                    new PuntoEmision
                    {
                        Codigo = int.Parse(f.InfoTributaria.PtoEmi),
                        Establecimiento = new Establecimiento
                        {
                            Codigo = int.Parse(f.InfoTributaria.Estab),
                            Emisor = new Emisor
                                { RUC = f.InfoTributaria.Ruc, EnumTipoAmbiente = tipoAmbiente }
                        }
                    }, long.Parse(f.InfoTributaria.Secuencial), EnumTipoEmision.Normal),
#pragma warning restore CA1305
                CodDoc = f.InfoTributaria.CodDoc,
                Estab = f.InfoTributaria.Estab,
                PtoEmi = f.InfoTributaria.PtoEmi,
                Secuencial = f.InfoTributaria.Secuencial,
                DirMatriz = f.InfoTributaria.DirMatriz
            },
            InfoFactura = new InfoFactura
            {
                FechaEmision = f.InfoFactura.FechaEmision,
                DirEstablecimiento = f.InfoFactura.DirEstablecimiento,
                ContribuyenteEspecial = f.InfoFactura.ContribuyenteEspecial,
                ObligadoContabilidad = f.InfoFactura.ObligadoContabilidad,
                TipoIdentificacionComprador = f.InfoFactura.TipoIdentificacionComprador,
                RazonSocialComprador = f.InfoFactura.RazonSocialComprador,
                IdentificacionComprador = f.InfoFactura.IdentificacionComprador,
                DireccionComprador = f.InfoFactura.DireccionComprador,
                TotalSinImpuestos = f.InfoFactura.TotalSinImpuestos,
                TotalDescuento = f.InfoFactura.TotalDescuento,
                TotalConImpuestos = f.InfoFactura.TotalConImpuestos.Select(i => new TotalImpuesto
                {
                    Codigo = i.Codigo,
                    CodigoPorcentaje = i.CodigoPorcentaje,
                    BaseImponible = i.BaseImponible,
                    Valor = i.Valor
                }).ToList(),
                Propina = f.InfoFactura.Propina,
                ImporteTotal = f.InfoFactura.ImporteTotal,
                Moneda = f.InfoFactura.Moneda,
                Pagos = f.InfoFactura.Pagos.Select(p => new Pago
                {
                    FormaPago = p.FormaPago,
                    Total = p.Total,
                    Plazo = p.Plazo,
                    UnidadTiempo = p.UnidadTiempo
                }).ToList(),
                ValorRetIva = f.InfoFactura.ValorRetIva,
                ValorRetRenta = f.InfoFactura.ValorRetRenta
            },
            Detalles = f.Detalles.Select(d => new Detalle
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

    public static XmlDocument ToXmlDocument(this XDocument xDocument)
    {
        var xmlDocument = new XmlDocument();
        using XmlReader reader = xDocument.CreateReader();
        xmlDocument.Load(reader);

        return xmlDocument;
    }
}
