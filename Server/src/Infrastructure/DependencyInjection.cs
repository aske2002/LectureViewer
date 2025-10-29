using backend.Application.Common.Interfaces;
using backend.Application.Common.Models;
using backend.Domain.Constants;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;
using backend.Infrastructure.Data;
using backend.Infrastructure.Data.Extensions;
using backend.Infrastructure.Data.Interceptors;
using backend.Infrastructure.Data.Resources;
using backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.Services.ConfigureRepositories();
        builder.Services.AddHttpClient("RestfulCountries", httpClient =>
        {
            var token = builder.Configuration.GetValue<string>("RestfulCountries:Token") ?? throw new ArgumentNullException("Token", "Token cannot be null. Please check your configuration.");
            var baseUrl = builder.Configuration.GetValue<string>("RestfulCountries:BaseUrl") ?? throw new ArgumentNullException("BaseUrl", "BaseUrl cannot be null. Please check your configuration.");
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        });
        builder.Services.AddTransient<IResourceFileManager, ResourceFileManager>();
        builder.Services.AddTransient<IResourceService, ResourceService>();

        var connectionString = builder.Configuration.GetConnectionString("BackendDB");
        Guard.Against.Null(connectionString, message: "Connection string 'BackendDB' not found.");

        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        builder.Services.ConfigureDataSeeders();

        builder.Services.AddLogging(options =>
        {
            options.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Debug);
            options.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Debug);
            options.AddFilter("Microsoft.EntityFrameworkCore.Database.Transaction", LogLevel.Debug);
            options.AddFilter("Microsoft.EntityFrameworkCore.Update", LogLevel.Debug);
        });

        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<ApplicationDbContextInitialiser>();


        builder.Services
            .AddDefaultIdentity<ApplicationUser>()
            .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddTransient<IIdentityService, IdentityService>();

        builder.Services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });
    }
}
