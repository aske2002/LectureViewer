using System.Text.Json.Serialization;
using Azure.Identity;
using backend.Application.Common.Interfaces;
using backend.Domain.Helpers;
using backend.Infrastructure.Data;
using backend.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NJsonSchema.Generation;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new StronglyTypedIdJsonConverterFactory());
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddScoped<IUser, CurrentUser>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.AddRazorPages();

        // Customise default API behaviour
        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiDocument((configure, sp) =>
        {
            configure.SchemaSettings.DefaultReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull;
            configure.AddStronglyTypedIdTypeMapper();
            configure.Title = "backend API";
        });


        builder.Services.AddOpenTelemetry()
        .ConfigureResource(resource => resource.AddService("base"))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter(
                    options => options.Endpoint = new Uri(Guard.Against.NullOrEmpty(builder.Configuration["OtelExporterOtlpEndpoint"]))
                    ))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(
                    options => options.Endpoint = new Uri(Guard.Against.NullOrEmpty(builder.Configuration["OtelExporterOtlpEndpoint"]))
                ));

    }

    public static void AddKeyVaultIfConfigured(this IHostApplicationBuilder builder)
    {
        var keyVaultUri = builder.Configuration["AZURE_KEY_VAULT_ENDPOINT"];
        if (!string.IsNullOrWhiteSpace(keyVaultUri))
        {
            builder.Configuration.AddAzureKeyVault(
                new Uri(keyVaultUri),
                new DefaultAzureCredential());
        }
    }
}
