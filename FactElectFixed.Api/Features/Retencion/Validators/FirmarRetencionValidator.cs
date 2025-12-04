using FactElectFixed.Api.Features.Retencion.Requests;
using FactElectFixed.Api.Helpers;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Validators;
using FastEndpoints;
using FluentValidation;

namespace FactElectFixed.Api.Features.Retencion.Validators;

public class RetencionValidator : Validator<FirmarDocumentoRequest<RetencionRequest>>
{
    public RetencionValidator(IValidator<RetencionRequest> comprobanteValidator)
    {
        Include(new FirmarDocumentoRequestValidator<RetencionRequest>(comprobanteValidator));
    }
}

public class RetencionRequestValidator : Validator<RetencionRequest>
{
    public RetencionRequestValidator(
        IValidator<InfoCompRetencion> infoCompRetencionValidator,
        IValidator<InfoTributariaRequest> infoTributariaValidator,
        IValidator<ImpuestoRetencionRequest> impuestoValidator)
    {
        RuleFor(x => x.InfoCompRetencion)
            .NotNull().WithMessage("'InfoCompRetencion' es obligatorio.")
            .SetValidator(infoCompRetencionValidator);

        RuleFor(x => x.InfoTributariaRequest)
            .NotNull().WithMessage("'InfoTributaria' es obligatorio.")
            .SetValidator(infoTributariaValidator);

        RuleFor(x => x.Impuestos)
            .NotNull().WithMessage("'DocsSustento' es obligatorio.")
            .NotEmpty().WithMessage("Debe existir al menos un 'DocSustentoRequest'.");

        RuleForEach(x => x.Impuestos)
            .SetValidator(impuestoValidator);
    }
}

public class InfoCompRetencionValidator : Validator<InfoCompRetencion>
{
    public InfoCompRetencionValidator()
    {
        RuleFor(x => x.FechaEmision)
            .NotEmpty().WithMessage("'FechaEmision' es obligatorio.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("'FechaEmision' no puede ser futura.");

        RuleFor(x => x.DirEstablecimiento).MaximumLength(300);

        RuleFor(x => x.ContribuyenteEspecial)
            .Length(3, 13).When(x => !string.IsNullOrWhiteSpace(x.ContribuyenteEspecial));

        RuleFor(x => x.ObligadoContabilidad)
            .NotNull()
            .WithMessage("ObligadoContabilidad debe ser 'SI' o 'NO'.");

        RuleFor(x => x.TipoIdentificacionSujetoRetenido)
            .NotEmpty().Length(2).Matches(@"^\d{2}$");

        RuleFor(x => x.RazonSocialSujetoRetenido).NotEmpty().MaximumLength(300);

        RuleFor(x => x.IdentificacionSujetoRetenido).NotEmpty().MaximumLength(20);

        RuleFor(x => x.PeriodoFiscal)
            .NotEmpty()
            .Must(ValidationHelpers.IsPeriod_MMyyyy)
            .WithMessage("PeriodoFiscal debe tener formato MM/yyyy.");
    }
}

public class ImpuestoRetencionRequestValidator : AbstractValidator<ImpuestoRetencionRequest>
{
    public ImpuestoRetencionRequestValidator()
    {
        RuleFor(x => x.Codigo).InclusiveBetween(1, 9);

        RuleFor(x => x.CodigoRetencion)
            .NotEmpty()
            .MaximumLength(5); 

        RuleFor(x => x.BaseImponible)
            .NotNull()
            .Must(d => ValidationHelpers.HasPrecisionAndScale(d, 14, 2))
            .WithMessage("BaseImponible excede precisión/escala (max 14, 2 decimales).");

        RuleFor(x => x.PorcentajeRetener)
            .NotNull()
            .Must(p => ValidationHelpers.HasPrecisionAndScale(p, 5, 2) || ValidationHelpers.HasPrecisionAndScale(p, 3, 0))
            .WithMessage("PorcentajeRetener inválido (max 5 dig. y hasta 2 decimales).");

        RuleFor(x => x.ValorRetenido)
            .NotNull()
            .Must(d => ValidationHelpers.HasPrecisionAndScale(d, 14, 2))
            .WithMessage("ValorRetenido excede precisión/escala (max 14, 2 decimales).");

        RuleFor(x => x.CodDocSustento)
            .NotEmpty()
            .Length(2)
            .Matches(@"^\d{2}$");

        RuleFor(x => x.NumDocSustento)
            .MaximumLength(15)
            .Matches(@"^[0-9\-]*$").When(x => !string.IsNullOrWhiteSpace(x.NumDocSustento));

        RuleFor(x => x.FechaEmisionDocSustento)
            .Must(d => string.IsNullOrWhiteSpace(d) || ValidationHelpers.IsDate_ddMMyyyy(d))
            .WithMessage("FechaEmisionDocSustento debe tener formato dd/MM/yyyy si se envía.");
    }
}
