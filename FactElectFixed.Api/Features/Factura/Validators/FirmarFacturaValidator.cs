using FactElectFixed.Api.Features.Factura.Requests;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Validators;
using FastEndpoints;
using FluentValidation;

namespace FactElectFixed.Api.Features.Factura.Validators;

public class FacturaValidator : Validator<FirmarDocumentoRequest<FacturaRequest>>
{
    public FacturaValidator(IValidator<FacturaRequest> comprobanteValidator,
#pragma warning disable IDE0060
        IValidator<InfoFactura> infoFacturaValidator, IValidator<InfoTributariaRequest> infoTributariaValidator,
        IValidator<DetalleRequest> detalleValidator)
#pragma warning restore IDE0060
    {
        Include(new FirmarDocumentoRequestValidator<FacturaRequest>(comprobanteValidator));
    }
}

public class FacturaRequestValidator : Validator<FacturaRequest>
{
    public FacturaRequestValidator(
        IValidator<InfoFactura> infoFacturaValidator,
        IValidator<InfoTributariaRequest> infoTributariaValidator,
        IValidator<DetalleRequest> detalleValidator)
    {
        RuleFor(x => x.InfoFactura)
            .NotNull().WithMessage("'InfoFacturaXml' es obligatorio.")
            .SetValidator(infoFacturaValidator);

        RuleFor(x => x.InfoTributariaRequest)
            .NotNull().WithMessage("'InfoTributariaXml' es obligatorio.")
            .SetValidator(infoTributariaValidator);

        RuleFor(x => x.Detalles)
            .NotNull().WithMessage("'Detalles' es obligatorio.")
            .NotEmpty().WithMessage("Debe existir al menos un detalle.");

        RuleForEach(x => x.Detalles)
            .SetValidator(detalleValidator);
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
