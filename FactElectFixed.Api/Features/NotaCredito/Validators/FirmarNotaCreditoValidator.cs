using FactElectFixed.Api.Features.Factura.Requests;
using FactElectFixed.Api.Features.NotaCredito.Requests;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Validators;
using FastEndpoints;
using FluentValidation;

namespace FactElectFixed.Api.Features.NotaCredito.Validators;

public class NotaCreditoValidator : Validator<FirmarDocumentoRequest<NotaCreditoRequest>>
{
    public NotaCreditoValidator(IValidator<NotaCreditoRequest> comprobanteValidator,
#pragma warning disable IDE0060
        IValidator<InfoNotaCredito> infoNotaCreditoValidator, IValidator<InfoTributaria> infoTributariaValidator,
        IValidator<Detalle> detalleValidator)
#pragma warning restore IDE0060
    {
        Include(new FirmarDocumentoRequestValidator<NotaCreditoRequest>(comprobanteValidator));
    }
}

public class NotaCreditoRequestValidator : Validator<NotaCreditoRequest>
{
    public NotaCreditoRequestValidator(
        IValidator<InfoNotaCredito> infoNotaCreditoValidator,
        IValidator<InfoTributaria> infoTributariaValidator,
        IValidator<Detalle> detalleValidator)
    {
        RuleFor(x => x.InfoNotaCredito)
            .NotNull().WithMessage("'InfoNotaCredito' es obligatorio.")
            .SetValidator(infoNotaCreditoValidator);

        RuleFor(x => x.InfoTributaria)
            .NotNull().WithMessage("'InfoTributaria' es obligatorio.")
            .SetValidator(infoTributariaValidator);

        RuleFor(x => x.Detalles)
            .NotNull().WithMessage("'Detalles' es obligatorio.")
            .NotEmpty().WithMessage("Debe existir al menos un detalle.");

        RuleForEach(x => x.Detalles)
            .SetValidator(detalleValidator);
    }
}

public class InfoNotaCreditoValidator : Validator<InfoNotaCredito>
{
    public InfoNotaCreditoValidator()
    {
        RuleFor(x => x.FechaEmision)
            .NotEmpty().WithMessage("El campo 'FechaEmision' es obligatorio.")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("El campo 'FechaEmision' no puede ser una fecha futura.");

        RuleFor(x => x.DirEstablecimiento)
            .MaximumLength(300)
            .WithMessage("El campo 'DirEstablecimiento' no puede tener más de 300 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.DirEstablecimiento));

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

        RuleFor(x => x.ContribuyenteEspecial)
            .Length(3, 13)
            .WithMessage("El campo 'ContribuyenteEspecial' debe tener entre 3 y 13 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.ContribuyenteEspecial));

        RuleFor(x => x.ObligadoContabilidad)
            .NotNull().WithMessage("El campo 'ObligadoContabilidad' es obligatorio (true/false).");

        RuleFor(x => x.Rise)
            .MaximumLength(40)
            .WithMessage("El campo 'Rise' no puede tener más de 40 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Rise));

        RuleFor(x => x.CodDocModificado)
            .NotEmpty().WithMessage("El campo 'CodDocModificado' es obligatorio.")
            .MaximumLength(2).WithMessage("El campo 'CodDocModificado' no puede tener más de 2 caracteres.");

        RuleFor(x => x.NumDocModificado)
            .NotEmpty().WithMessage("El campo 'NumDocModificado' es obligatorio.")
            .MaximumLength(17).WithMessage("El campo 'NumDocModificado' no puede tener más de 17 caracteres.");

        RuleFor(x => x.FechaEmisionDocSustento)
            .NotEmpty().WithMessage("El campo 'FechaEmisionDocSustento' es obligatorio.")
            .MaximumLength(10).WithMessage("El campo 'FechaEmisionDocSustento' no puede tener más de 10 caracteres.");

        RuleFor(x => x.TotalSinImpuestos)
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'TotalSinImpuestos' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.ValorModificacion)
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'ValorModificacion' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.Moneda)
            .MaximumLength(15)
            .WithMessage("El campo 'Moneda' no puede tener más de 15 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Moneda));

        RuleFor(x => x.TotalConImpuestos)
            .NotNull().WithMessage("La lista 'TotalConImpuestos' es obligatoria.")
            .NotEmpty().WithMessage("Debe existir al menos un elemento en 'TotalConImpuestos'.");

        RuleForEach(x => x.TotalConImpuestos)
            .SetValidator(new TotalImpuestoValidator());

        RuleFor(x => x.Motivo)
            .NotEmpty().WithMessage("El campo 'Motivo' es obligatorio.")
            .MaximumLength(300)
            .WithMessage("El campo 'Motivo' no puede tener más de 300 caracteres.");
    }
}
