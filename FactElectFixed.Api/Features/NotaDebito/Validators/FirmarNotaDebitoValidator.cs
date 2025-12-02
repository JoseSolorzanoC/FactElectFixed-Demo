using FactElectFixed.Api.Features.NotaDebito.Requests;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Validators;
using FastEndpoints;
using FluentValidation;

namespace FactElectFixed.Api.Features.NotaDebito.Validators;

public class NotaDebitoValidator : Validator<FirmarDocumentoRequest<NotaDebitoRequest>>
{
    public NotaDebitoValidator(IValidator<NotaDebitoRequest> comprobanteValidator,
#pragma warning disable IDE0060
        IValidator<InfoNotaDebitoRequest> infoNotaCreditoValidator,
        IValidator<InfoTributariaRequest> infoTributariaValidator,
        IValidator<DetalleRequest> detalleValidator)
#pragma warning restore IDE0060
    {
        Include(new FirmarDocumentoRequestValidator<NotaDebitoRequest>(comprobanteValidator));
    }
}

public class NotaDebitoRequestValidator : Validator<NotaDebitoRequest>
{
    public NotaDebitoRequestValidator(
        IValidator<InfoNotaDebitoRequest> infoNotaDebitoValidator,
        IValidator<InfoTributariaRequest> infoTributariaValidator,
        IValidator<MotivoRequest> motivoValidator)
    {
        RuleFor(x => x.InfoNotaDebitoRequest)
            .NotNull().WithMessage("'InfoNotaDebitoRequest' es obligatorio.")
            .SetValidator(infoNotaDebitoValidator);

        RuleFor(x => x.InfoTributariaRequest)
            .NotNull().WithMessage("'InfoTributariaXml' es obligatorio.")
            .SetValidator(infoTributariaValidator);

        RuleFor(x => x.Motivos)
            .NotNull().WithMessage("'Motivos' es obligatorio.")
            .NotEmpty().WithMessage("Debe existir al menos un motivo.");

        RuleForEach(x => x.Motivos)
            .SetValidator(motivoValidator);
    }
}

public class InfoNotaDebitoValidator : Validator<InfoNotaDebitoRequest>
{
    public InfoNotaDebitoValidator(
        IValidator<ImpuestoRequest> impuestoValidator, IValidator<PagoRequest> pagoValidator
    )
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
            .MaximumLength(2).WithMessage("El campo 'TipoIdentificacionComprador' no puede tener más de 2 caracteres.");

        RuleFor(x => x.RazonSocialComprador)
            .NotEmpty().WithMessage("El campo 'RazonSocialComprador' es obligatorio.")
            .MaximumLength(300).WithMessage("El campo 'RazonSocialComprador' no puede tener más de 300 caracteres.");

        RuleFor(x => x.IdentificacionComprador)
            .NotEmpty().WithMessage("El campo 'IdentificacionComprador' es obligatorio.")
            .MaximumLength(20).WithMessage("El campo 'IdentificacionComprador' no puede tener más de 20 caracteres.");

        RuleFor(x => x.ContribuyenteEspecial)
            .Length(3, 13)
            .WithMessage("El campo 'ContribuyenteEspecial' debe tener entre 3 y 13 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.ContribuyenteEspecial));

        RuleFor(x => x.ObligadoContabilidad)
            .NotNull().WithMessage("El campo 'ObligadoContabilidad' es obligatorio.");

        RuleFor(x => x.CodDocModificado)
            .NotEmpty().WithMessage("El campo 'CodDocModificado' es obligatorio.")
            .MaximumLength(2).WithMessage("El campo 'CodDocModificado' no puede tener más de 2 caracteres.");

        RuleFor(x => x.NumDocModificado)
            .NotEmpty().WithMessage("El campo 'NumDocModificado' es obligatorio.")
            .MaximumLength(17).WithMessage("El campo 'NumDocModificado' no puede tener más de 17 caracteres.");

        RuleFor(x => x.FechaEmisionDocSustento)
            .NotEmpty().WithMessage("El campo 'FechaEmisionDocSustento' es obligatorio.")
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("El campo 'FechaEmisionDocSustento' no puede ser una fecha futura.");

        RuleFor(x => x.TotalSinImpuestos)
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'TotalSinImpuestos' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.ValorTotal)
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'ValorTotal' debe estar entre 0 y 999999999999.99.");

        RuleFor(x => x.Impuestos)
            .NotNull().WithMessage("La lista 'Impuestos' es obligatoria.")
            .NotEmpty().WithMessage("Debe existir al menos un elemento en 'Impuestos'.");

        RuleForEach(x => x.Impuestos)
            .SetValidator(impuestoValidator);

        RuleFor(x => x.Pagos)
            .NotNull().WithMessage("La lista 'Pagos' es obligatoria.")
            .NotEmpty().WithMessage("Debe existir al menos un elemento en 'Pagos'.");

        RuleForEach(x => x.Pagos)
            .SetValidator(pagoValidator);
    }
}

public class MotivoValidator : Validator<MotivoRequest>
{
    public MotivoValidator()
    {
        RuleFor(x => x.Razon)
            .NotEmpty().WithMessage("El campo 'Razon' es obligatorio.")
            .MaximumLength(300)
            .WithMessage("El campo 'Razon' no puede exceder 300 caracteres.");

        RuleFor(x => x.Valor)
            .InclusiveBetween(0, 999999999999.99M)
            .WithMessage("El campo 'Valor' debe estar entre 0 y 999999999999.99.");
    }
}
