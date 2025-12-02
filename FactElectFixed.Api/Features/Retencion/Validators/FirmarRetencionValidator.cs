using FactElectFixed.Api.Features.Retencion.Requests;
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
        IValidator<InfoCompRetencionRequest> infoCompRetencionValidator,
        IValidator<InfoTributariaRequest> infoTributariaValidator,
        IValidator<DocSustentoRequest> docSustentoValidator)
    {
        RuleFor(x => x.InfoCompRetencionRequest)
            .NotNull().WithMessage("'InfoCompRetencionRequest' es obligatorio.")
            .SetValidator(infoCompRetencionValidator);

        RuleFor(x => x.InfoTributariaRequest)
            .NotNull().WithMessage("'InfoTributariaXml' es obligatorio.")
            .SetValidator(infoTributariaValidator);

        RuleFor(x => x.DocsSustento)
            .NotNull().WithMessage("'DocsSustento' es obligatorio.")
            .NotEmpty().WithMessage("Debe existir al menos un 'DocSustentoRequest'.");

        RuleForEach(x => x.DocsSustento)
            .SetValidator(docSustentoValidator);

        // RuleForEach(x => x.InfoAdicional)
        //     .SetValidator(new CampoAdicionalVali)
        //     .When(x => x.InfoAdicional != null && x.InfoAdicional.Any());
    }
}

public class InfoCompRetencionValidator : Validator<InfoCompRetencionRequest>
{
    public InfoCompRetencionValidator()
    {
        RuleFor(x => x.FechaEmision)
            .NotEmpty().WithMessage("'FechaEmision' es obligatorio.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("'FechaEmision' no puede ser futura.");

        RuleFor(x => x.TipoIdentificacionSujetoRetenido)
            .NotEmpty().WithMessage("'TipoIdentificacionSujetoRetenido' es obligatorio.")
            .MaximumLength(2);

        RuleFor(x => x.RazonSocialSujetoRetenido)
            .NotEmpty().WithMessage("'RazonSocialSujetoRetenido' es obligatorio.")
            .MaximumLength(300);

        RuleFor(x => x.IdentificacionSujetoRetenido)
            .NotEmpty().WithMessage("'IdentificacionSujetoRetenido' es obligatorio.")
            .MaximumLength(20);

        RuleFor(x => x.PeriodoFiscal)
            .NotEmpty().WithMessage("'PeriodoFiscal' es obligatorio.")
            .Matches(@"^(0[1-9]|1[0-2])\/\d{4}$").WithMessage("'PeriodoFiscal' debe tener formato mm/yyyy.");

        RuleFor(x => x.ContribuyenteEspecial)
            .Length(3, 13)
            .When(x => !string.IsNullOrWhiteSpace(x.ContribuyenteEspecial));

        RuleFor(x => x.ObligadoContabilidad)
            .NotNull().WithMessage("'ObligadoContabilidad' es requerido cuando aplica.");
    }
}

public class DocSustentoValidator : Validator<DocSustentoRequest>
{
    public DocSustentoValidator(IValidator<ImpuestoDocSustentoRequest> impuestoValidator,
        IValidator<RetencionDetalleRequest> retencionValidator, IValidator<PagoRequest> pagoValidator)
    {
        RuleFor(x => x.CodSustento)
            .NotEmpty().WithMessage("'CodSustento' es obligatorio.");

        RuleFor(x => x.CodDocSustento)
            .NotEmpty().WithMessage("'CodDocSustento' es obligatorio.");

        RuleFor(x => x.FechaEmisionDocSustento)
            .NotEmpty().WithMessage("'FechaEmisionDocSustento' es obligatorio.");

        RuleFor(x => x.TotalSinImpuestos)
            .InclusiveBetween(0, 999999999999.99M);

        RuleFor(x => x.ImporteTotal)
            .InclusiveBetween(0, 999999999999.99M);

        RuleForEach(x => x.ImpuestosDocSustento).SetValidator(impuestoValidator);

        RuleForEach(x => x.Retenciones).SetValidator(retencionValidator);

        RuleForEach(x => x.Pagos).SetValidator(pagoValidator);

        // if (reembolsoValidator != null)
        // {
        //     RuleFor(x => x.ReembolsoDetalleRequest).SetValidator(reembolsoValidator).When(x => x.ReembolsoDetalleRequest != null);
        // }
    }
}

public class ImpuestoDocSustentoValidator : Validator<ImpuestoDocSustentoRequest>
{
    public ImpuestoDocSustentoValidator()
    {
        RuleFor(x => x.CodImpuestoDocSustento).NotEmpty();
        RuleFor(x => x.CodigoPorcentaje).NotEmpty();
        RuleFor(x => x.BaseImponible).InclusiveBetween(0, 999999999999.99M);
        RuleFor(x => x.Tarifa).InclusiveBetween(0, 9999.99M);
        RuleFor(x => x.ValorImpuesto).InclusiveBetween(0, 999999999999.99M);
    }
}

public class RetencionDetalleValidator : Validator<RetencionDetalleRequest>
{
    public RetencionDetalleValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty();
        RuleFor(x => x.CodigoRetencion).NotEmpty();
        RuleFor(x => x.BaseImponible).InclusiveBetween(0, 999999999999.99M);
        RuleFor(x => x.PorcentajeRetener).InclusiveBetween(0, 100M);
        RuleFor(x => x.ValorRetenido).InclusiveBetween(0, 999999999999.99M);
    }
}
