using System.Diagnostics;
using System.Text.Json.Serialization;
using FactElectFixed.Api.Database;
using FactElectFixed.Api.Features.Configuracion.Services;
using FactElectFixed.Api.Features.Factura.Services.FirmarFactura;
using FactElectFixed.Api.Middlewares;
using FactElectFixed.Api.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
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

builder.Services.AddTransient<IFirmarFacturaService, FirmarFacturaService>();
builder.Services.AddTransient<IConfiguracionService, ConfiguracionService>();
builder.Services.AddSingleton<ICachedXmlFileService, CachedXmlFileService>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

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
