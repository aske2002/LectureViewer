using backend.Application.Common.Interfaces;
using backend.Domain.Constants;
using backend.Domain.Entities;
using backend.Infrastructure.Background;
using backend.Infrastructure.Data;
using backend.Infrastructure.Data.Extensions;
using backend.Infrastructure.Data.Interceptors;
using backend.Infrastructure.Data.Resources;
using backend.Infrastructure.Identity;
using backend.Infrastructure.Identity.AuthorizationHandlers;
using backend.Infrastructure.MediaProcessing;
using backend.Infrastructure.MediaProcessing.Configurations;
using backend.Infrastructure.MediaProcessing.Transcription;
using backend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        builder.AddMediaProcessing();

        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.AllowOutOfOrderMetadataProperties = true;
        });
        // Settings
        builder.Services.Configure<TranscodingConfiguration>(
            builder.Configuration.GetSection("Transcoding"));
        builder.Services.Configure<TranscriptionConfiguration>(
            builder.Configuration.GetSection("Transcription"));
        builder.Services.Configure<DocumentConversionConfiguration>(
            builder.Configuration.GetSection("DocumentConversion"));
            
        builder.Services.AddHttpClient<IDocumentService, DocumentService>()
            .ConfigureHttpClient((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<DocumentConversionConfiguration>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
            });
        builder.Services.AddHttpClient<IMediaService, MediaService>()
            .ConfigureHttpClient((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<TranscodingConfiguration>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
            });

        // Application services
        builder.Services.ConfigureRepositories();
        builder.Services.AddHttpClient("RestfulCountries", httpClient =>
        {
            var token = builder.Configuration.GetValue<string>("RestfulCountries:Token") ?? throw new ArgumentNullException("Token", "Token cannot be null. Please check your configuration.");
            var baseUrl = builder.Configuration.GetValue<string>("RestfulCountries:BaseUrl") ?? throw new ArgumentNullException("BaseUrl", "BaseUrl cannot be null. Please check your configuration.");
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        });
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddTransient<IResourceFileManager, ResourceFileManager>();
        builder.Services.AddTransient<IResourceService, ResourceService>();
        builder.Services.AddTransient<ICourseService, CourseService>();

        // Database setup
        var connectionString = builder.Configuration.GetConnectionString("BackendDB");
        Guard.Against.Null(connectionString, message: "Connection string 'BackendDB' not found.");

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        builder.Services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString).EnableSensitiveDataLogging();
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            options.LogTo(Console.WriteLine, LogLevel.Information);
        });
        builder.Services.ConfigureDataSeeders();
        builder.Services.AddLogging(options =>
        {
            // options.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Debug);
            // options.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Debug);
            // options.AddFilter("Microsoft.EntityFrameworkCore.Database.Transaction", LogLevel.Debug);
            // options.AddFilter("Microsoft.EntityFrameworkCore.Update", LogLevel.Debug);
        });
        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        // Identity setup
        builder.Services
            .AddDefaultIdentity<ApplicationUser>()
            .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddScoped<IUserAccessor, UserAccessor>();
        builder.Services.AddScoped<IIdentityService, IdentityService>();

        // Authorization handlers
        builder.Services.AddScoped<IAuthorizationHandler, CoursePermissionHandler>();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.CanCreateCourses, policy => policy.RequireRole(Roles.Instructor, Roles.Administrator));
        });
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
