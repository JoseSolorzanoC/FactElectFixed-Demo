using FactElectFixed.Api.Features.Factura.Requests;
using FastEndpoints;
using FluentValidation;

namespace FactElectFixed.Api.Features.Factura.Validators;

public class FirmarFacturaValidator : Validator<FirmarFacturaRequest>
{
    public FirmarFacturaValidator()
    {
        RuleForEach(ff => ff.Comprobantes).SetValidator(new FacturaValidator());
    }
}

public class FacturaValidator : Validator<Requests.Factura>
{
    public FacturaValidator()
    {
        RuleFor(f => f.InfoFactura).SetValidator(new InfoFacturaValidator());
        RuleForEach(f => f.Detalles).SetValidator(new DetalleValidator());
        RuleFor(f => f.InfoTributaria).SetValidator(new InfoTributariaValidator());
    }
}

public class InfoTributariaValidator : Validator<InfoTributaria>
{
    public InfoTributariaValidator()
    {
        RuleFor(x => x.Ambiente)
            .NotEmpty().WithMessage("El campo 'Ambiente' es obligatorio.")
            .InclusiveBetween(1, 2).WithMessage("El campo 'Ambiente' debe estar entre 1 y 2.");

        RuleFor(x => x.TipoEmision)
            .NotEmpty().WithMessage("El campo 'TipoEmision' es obligatorio.")
            .Equal(1).WithMessage("El campo 'TipoEmision' debe ser igual a 1.");

        RuleFor(x => x.RazonSocial)
            .NotEmpty().WithMessage("El campo 'RazonSocial' es obligatorio.")
            .MaximumLength(300).WithMessage("El campo 'RazonSocial' no puede tener más de 300 caracteres.");

        RuleFor(x => x.NombreComercial)
            .MaximumLength(300).WithMessage("El campo 'NombreComercial' no puede tener más de 300 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.NombreComercial));

        RuleFor(x => x.Ruc)
            .NotEmpty().WithMessage("El campo 'Ruc' es obligatorio.")
            .Length(13).WithMessage("El campo 'Ruc' debe tener exactamente 13 caracteres.");

        RuleFor(x => x.CodDoc)
            .NotEmpty().WithMessage("El campo 'CodDoc' es obligatorio.")
            .MaximumLength(2).WithMessage("El campo 'CodDoc' no puede tener más de 2 caracteres.");

        RuleFor(x => x.Estab)
            .NotEmpty().WithMessage("El campo 'Estab' es obligatorio.")
            .MaximumLength(3).WithMessage("El campo 'Estab' no puede tener más de 3 caracteres.");

        RuleFor(x => x.PtoEmi)
            .NotEmpty().WithMessage("El campo 'PtoEmi' es obligatorio.")
            .MaximumLength(3).WithMessage("El campo 'PtoEmi' no puede tener más de 3 caracteres.");

        RuleFor(x => x.Secuencial)
            .NotEmpty().WithMessage("El campo 'Secuencial' es obligatorio.")
            .MaximumLength(9).WithMessage("El campo 'Secuencial' no puede tener más de 9 caracteres.");

        RuleFor(x => x.DirMatriz)
            .NotEmpty().WithMessage("El campo 'DirMatriz' es obligatorio.")
            .MaximumLength(300).WithMessage("El campo 'DirMatriz' no puede tener más de 300 caracteres.");
    }
}

public class InfoFacturaValidator : Validator<InfoFactura>
{
    public InfoFacturaValidator()
    {
        RuleFor(x => x.FechaEmision)
            .NotEmpty().WithMessage("El campo 'FechaEmision' es obligatorio.")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("El campo 'FechaEmision' no puede ser una fecha futura.");

        RuleFor(x => x.DirEstablecimiento)
            .MaximumLength(300)
            .WithMessage("El campo 'DirEstablecimiento' no puede tener más de 300 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.DirEstablecimiento));

        RuleFor(x => x.ContribuyenteEspecial)
            .Length(3, 13)
            .WithMessage("El campo 'ContribuyenteEspecial' debe tener entre 3 y 13 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.ContribuyenteEspecial));

        RuleFor(x => x.ObligadoContabilidad)
            .NotNull()
            .WithMessage("El campo 'ObligadoContabilidad' es obligatorio (true/false).");

        RuleFor(x => x.TipoIdentificacionComprador)
            .NotEmpty().WithMessage("El campo 'TipoIdentificacionComprador' es obligatorio.")
            .MaximumLength(2)
            .WithMessage("El campo 'TipoIdentificacionComprador' no puede tener más de 2 caracteres.");

        RuleFor(x => x.RazonSocialComprador)
            .NotEmpty().WithMessage("El campo 'RazonSocialComprador' es obligatorio.")
            .MaximumLength(300)
            .WithMessage("El campo 'RazonSocialComprador' no puede tener más de 300 caracteres.");

        RuleFor(x => x.IdentificacionComprador)
            .NotEmpty().WithMessage("El campo 'IdentificacionComprador' es obligatorio.")
            .MaximumLength(13)
            .WithMessage("El campo 'IdentificacionComprador' no puede tener más de 13 caracteres.");

        RuleFor(x => x.DireccionComprador)
            .MaximumLength(300)
            .WithMessage("El campo 'DireccionComprador' no puede tener más de 300 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.DireccionComprador));

        RuleFor(x => x.TotalSinImpuestos)
            .NotNull().WithMessage("El campo 'TotalSinImpuestos' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'TotalSinImpuestos' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.TotalDescuento)
            .NotNull().WithMessage("El campo 'TotalDescuento' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'TotalDescuento' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.TotalConImpuestos)
            .NotNull().WithMessage("La lista 'TotalConImpuestos' es obligatoria.")
            .NotEmpty().WithMessage("Debe existir al menos un elemento en 'TotalConImpuestos'.");

        RuleForEach(x => x.TotalConImpuestos)
            .SetValidator(new TotalImpuestoValidator());

        RuleFor(x => x.Propina)
            .NotNull().WithMessage("El campo 'Propina' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'Propina' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.ImporteTotal)
            .NotNull().WithMessage("El campo 'ImporteTotal' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'ImporteTotal' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.Moneda)
            .MaximumLength(15)
            .WithMessage("El campo 'Moneda' no puede tener más de 15 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Moneda));

        RuleFor(x => x.Pagos)
            .NotNull().WithMessage("La lista 'Pagos' es obligatoria.")
            .NotEmpty().WithMessage("Debe existir al menos un elemento en 'Pagos'.");

        RuleForEach(x => x.Pagos)
            .SetValidator(new PagoValidator());

        RuleFor(x => x.ValorRetIva)
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'ValorRetIva' debe estar entre 0 y 999999999999.99.")
            .When(x => x.ValorRetIva.HasValue);

        RuleFor(x => x.ValorRetRenta)
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'ValorRetRenta' debe estar entre 0 y 999999999999.99.")
            .When(x => x.ValorRetRenta.HasValue);
    }
}

public class TotalImpuestoValidator : Validator<TotalImpuesto>
{
    private static readonly int[] AllowedCodigoImpuesto = [2, 3, 5];

    public TotalImpuestoValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El campo 'Codigo' es obligatorio.")
            .Must(c => AllowedCodigoImpuesto.Contains(c))
            .WithMessage("El campo 'Codigo' solo puede tener los valores 2, 3 o 5.");

        RuleFor(x => x.CodigoPorcentaje)
            .NotEmpty().WithMessage("El campo 'CodigoPorcentaje' es obligatorio.")
            .InclusiveBetween(1, 9999)
            .WithMessage("El campo 'CodigoPorcentaje' debe estar entre 1 y 9999.");

        RuleFor(x => x.BaseImponible)
            .NotNull().WithMessage("El campo 'BaseImponible' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'BaseImponible' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.Valor)
            .NotNull().WithMessage("El campo 'Valor' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'Valor' debe estar entre 0 y 999999999999.99.");
    }
}

public class PagoValidator : Validator<Pago>
{
    public PagoValidator()
    {
        RuleFor(x => x.FormaPago)
            .NotEmpty().WithMessage("El campo 'FormaPago' es obligatorio.")
            .MaximumLength(2).WithMessage("El campo 'FormaPago' no puede tener más de 2 caracteres.");

        RuleFor(x => x.Total)
            .NotNull().WithMessage("El campo 'Total' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'Total' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.Plazo)
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'Plazo' debe estar entre 0 y 999999999999.99.")
            .When(x => x.Plazo != default)
            .WithMessage("El campo 'Plazo' es opcional, pero debe ser válido si se incluye.");

        RuleFor(x => x.UnidadTiempo)
            .MaximumLength(10)
            .WithMessage("El campo 'UnidadTiempo' no puede tener más de 10 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.UnidadTiempo));
    }
}

public class DetalleValidator : Validator<Detalle>
{
    public DetalleValidator()
    {
        RuleFor(x => x.CodigoPrincipal)
            .NotEmpty().WithMessage("El campo 'CodigoPrincipal' es obligatorio.")
            .MaximumLength(25).WithMessage("El campo 'CodigoPrincipal' no puede tener más de 25 caracteres.");

        RuleFor(x => x.CodigoAuxiliar)
            .MaximumLength(25)
            .WithMessage("El campo 'CodigoAuxiliar' no puede tener más de 25 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.CodigoAuxiliar));

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("El campo 'Descripcion' es obligatorio.")
            .MaximumLength(300).WithMessage("El campo 'Descripcion' no puede tener más de 300 caracteres.");

        RuleFor(x => x.Cantidad)
            .NotNull().WithMessage("El campo 'Cantidad' es obligatorio.")
            .InclusiveBetween(0, 999999999999.999999M)
            .PrecisionScale(18, 6, false)
            .WithMessage("El campo 'Cantidad' debe estar entre 0 y 999999999999.999999.");

        RuleFor(x => x.PrecioUnitario)
            .NotNull().WithMessage("El campo 'PrecioUnitario' es obligatorio.")
            .InclusiveBetween(0, 999999999999.999999M)
            .PrecisionScale(18, 6, false)
            .WithMessage("El campo 'PrecioUnitario' debe estar entre 0 y 999999999999.999999.");

        RuleFor(x => x.Descuento)
            .NotNull().WithMessage("El campo 'Descuento' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .PrecisionScale(18, 6, false)
            .WithMessage("El campo 'Descuento' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.PrecioTotalSinImpuesto)
            .NotNull().WithMessage("El campo 'PrecioTotalSinImpuesto' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .PrecisionScale(18, 6, false)
            .WithMessage("El campo 'PrecioTotalSinImpuesto' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.Impuestos)
            .NotNull().WithMessage("La lista 'Impuestos' es obligatoria.")
            .NotEmpty().WithMessage("Debe especificar al menos un elemento en 'Impuestos'.");

        RuleForEach(x => x.Impuestos)
            .SetValidator(new ImpuestoValidator());
    }
}

public class ImpuestoValidator : Validator<Impuesto>
{
    private static readonly int[] AllowedCodigoImpuesto = [2, 3, 5];

    public ImpuestoValidator()
    {
        RuleFor(x => x.Codigo)
            .NotEmpty().WithMessage("El campo 'Codigo' es obligatorio.")
            .Must(c => AllowedCodigoImpuesto.Contains(c))
            .WithMessage("El campo 'Codigo' solo puede tener los valores 2, 3 o 5.");

        RuleFor(x => x.CodigoPorcentaje)
            .NotEmpty().WithMessage("El campo 'CodigoPorcentaje' es obligatorio.")
            .InclusiveBetween(1, 9999)
            .WithMessage("El campo 'CodigoPorcentaje' debe estar entre 1 y 9999.");

        RuleFor(x => x.Tarifa)
            .NotNull().WithMessage("El campo 'Tarifa' es obligatorio.")
            .InclusiveBetween(0.01M, 9999.99M)
            .WithMessage("El campo 'Tarifa' debe estar entre 0.01 y 9999.99.");

        RuleFor(x => x.BaseImponible)
            .NotNull().WithMessage("El campo 'BaseImponible' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'BaseImponible' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.Valor)
            .NotNull().WithMessage("El campo 'Valor' es obligatorio.")
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'Valor' debe estar entre 0 y 999999999999.99.");
    }
}
