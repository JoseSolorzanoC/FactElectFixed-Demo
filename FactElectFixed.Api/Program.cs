using System.Diagnostics;
using FactElectFixed.Api.Features.Factura.Firma.Services.FirmarFactura;
using FactElectFixed.Api.Helpers;
using FactElectFixed.Api.Middlewares;
using FactElectFixed.Api.Services;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infoware.SRI.DocumentosElectronicos.Configuracion;
using Infoware.SRI.WebService;
using Microsoft.AspNetCore.Http.Features;
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

builder.Services.AddFusionCache();

builder.Services.AddTransient<IFirmarFacturaService, FirmarFacturaService>();
builder.Services.AddSingleton<ICachedXmlFileService, CachedXmlFileService>();

WebApplication app = builder.Build();

app.UseMiddleware<IpWhitelistMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStatusCodePages();

app.UseHttpsRedirection();

app.UseFastEndpoints().UseSwaggerGen();

await app.RunAsync();
