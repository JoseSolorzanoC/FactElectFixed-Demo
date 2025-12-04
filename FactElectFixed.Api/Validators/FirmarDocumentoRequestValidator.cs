using FactElectFixed.Api.Requests;
using FastEndpoints;
using FluentValidation;

namespace FactElectFixed.Api.Validators;

public class FirmarDocumentoRequestValidator<T> : Validator<FirmarDocumentoRequest<T>> where T : class
{
    public FirmarDocumentoRequestValidator(IValidator<T> comprobanteValidator)
    {
        RuleFor(x => x.Ambiente)
            .IsInEnum()
            .WithMessage("El campo 'Ambiente' debe ser un valor válido.");

        RuleFor(x => x.Version)
            .NotEmpty()
            .WithMessage("El campo 'Version' es obligatorio.");

        RuleFor(x => x.Comprobantes)
            .NotNull()
            .WithMessage("La lista 'Comprobantes' es obligatoria.")
            .NotEmpty()
            .WithMessage("Debe existir al menos un comprobante.");

        RuleForEach(x => x.Comprobantes)
            .SetValidator(comprobanteValidator);
    }
}

public class InfoTributariaValidator : Validator<InfoTributariaRequest>
{
    public InfoTributariaValidator()
    {
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

public class TotalImpuestoValidator : Validator<TotalImpuestoRequest>
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

public class PagoValidator : Validator<PagoRequest>
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

public class DetalleValidator : Validator<DetalleRequest>
{
    public DetalleValidator()
    {
        RuleFor(x => x.CodigoPrincipal)
            .NotEmpty().WithMessage("El campo 'CodigoPrincipal' es obligatorio.")
            .MaximumLength(25).WithMessage("El campo 'CodigoPrincipal' no puede tener más de 25 caracteres.");

        RuleFor(x => x.CodigoAuxiliar)
            .MinimumLength(1)
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

public class ImpuestoValidator : Validator<ImpuestoRequest>
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
            .InclusiveBetween(0, 9999.99M)
            .WithMessage("El campo 'Tarifa' debe estar entre 0 y 9999.99.");

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
