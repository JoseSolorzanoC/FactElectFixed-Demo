using System.Diagnostics;
using System.Text.Json.Serialization;
using FactElectFixed.Api.Database;
using FactElectFixed.Api.Features.Configuracion.Services;
using FactElectFixed.Api.Features.Factura.Requests;
using FactElectFixed.Api.Features.Factura.Validators;
using FactElectFixed.Api.Features.NotaCredito.Requests;
using FactElectFixed.Api.Features.NotaCredito.Validators;
using FactElectFixed.Api.Features.NotaDebito.Requests;
using FactElectFixed.Api.Features.NotaDebito.Validators;
using FactElectFixed.Api.Features.Retencion.Requests;
using FactElectFixed.Api.Features.Retencion.Validators;
using FactElectFixed.Api.Middlewares;
using FactElectFixed.Api.Requests;
using FactElectFixed.Api.Services;
using FactElectFixed.Api.Services.FirmarDocumento;
using FactElectFixed.Api.Validators;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using Infoware.SRI.DocumentosElectronicos.Configuracion;
using Infoware.SRI.Firmar;
using Infoware.SRI.WebService;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints().SwaggerDocument();

builder.Services.AddTransient<IValidator<InfoFactura>, InfoFacturaValidator>();
builder.Services.AddTransient<IValidator<InfoNotaCreditoRequest>, InfoNotaCreditoValidator>();
builder.Services.AddTransient<IValidator<InfoNotaDebitoRequest>, InfoNotaDebitoValidator>();
builder.Services.AddTransient<IValidator<InfoCompRetencionRequest>, InfoCompRetencionValidator>();

builder.Services.AddTransient<IValidator<InfoTributariaRequest>, InfoTributariaValidator>();
builder.Services.AddTransient<IValidator<DetalleRequest>, DetalleValidator>();
builder.Services.AddTransient<IValidator<TotalImpuestoRequest>, TotalImpuestoValidator>();
builder.Services.AddTransient<IValidator<PagoRequest>, PagoValidator>();
builder.Services.AddTransient<IValidator<ImpuestoRequest>, ImpuestoValidator>();
builder.Services.AddTransient<IValidator<ImpuestoDocSustentoRequest>, ImpuestoDocSustentoValidator>();
builder.Services.AddTransient<IValidator<RetencionDetalleRequest>, RetencionDetalleValidator>();
builder.Services.AddTransient<IValidator<DocSustentoRequest>, DocSustentoValidator>();
builder.Services.AddTransient<IValidator<DetalleNotaCreditoRequest>, DetalleNotaCreditoValidator>();
builder.Services.AddTransient<IValidator<DetAdicionalRequest>, DetAdicionalRequestValidator>();
builder.Services.AddTransient<IValidator<MotivoRequest>, MotivoValidator>();


builder.Services.AddTransient<IValidator<FacturaRequest>, FacturaRequestValidator>();
builder.Services.AddTransient<IValidator<NotaCreditoRequest>, NotaCreditoRequestValidator>();
builder.Services.AddTransient<IValidator<NotaDebitoRequest>, NotaDebitoRequestValidator>();
builder.Services.AddTransient<IValidator<RetencionRequest>, RetencionRequestValidator>();

builder.Services.AddTransient<Validator<FirmarDocumentoRequest<FacturaRequest>>, FacturaValidator>();
builder.Services.AddTransient<Validator<FirmarDocumentoRequest<NotaCreditoRequest>>, NotaCreditoValidator>();
builder.Services.AddTransient<Validator<FirmarDocumentoRequest<NotaDebitoRequest>>, NotaDebitoValidator>();
builder.Services.AddTransient<Validator<FirmarDocumentoRequest<RetencionRequest>>, RetencionValidator>();

builder.Services.AddTransient(typeof(IValidator<>), typeof(FirmarDocumentoRequestValidator<>));

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

builder.Services.AddOpenApi();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
    .WithTracing(tracing => tracing
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation())
    .WithMetrics(metrics => metrics
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation())
    .UseOtlpExporter();

builder.Services.AddOptions<SRIDocumentosElectronicosOptions>()
    .BindConfiguration("SRIDocumentosElectronicosOptions")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<ISRIWebService, SRIWebService>();
builder.Services.AddSingleton<ICertificadoService, CertificadoService>();

builder.Services.AddFusionCache();

builder.Services.AddTransient<IFirmarDocumentoService, FirmarDocumentoService>();
builder.Services.AddTransient<IConfiguracionService, ConfiguracionService>();
builder.Services.AddSingleton<ICachedXmlFileService, CachedXmlFileService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

WebApplication app = builder.Build();

app.UseFastEndpoints(config =>
{
    config.Validation.EnableDataAnnotationsSupport = true;
    config.Errors.UseProblemDetails();
    config.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
}).UseSwaggerGen();

app.UseMiddleware<IpWhitelistMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

app.UseStatusCodePages();

app.UseHttpsRedirection();

await app.RunAsync();
