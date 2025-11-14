using backend.Domain.Extensions;
using backend.Infrastructure.Data;
using Microsoft.AspNetCore.Http.Features;


EntityTypeExtensions.Initialize();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();

builder.AddInfrastructureServices();
builder.AddWebServices();

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = 536870912; // 512 MB
    options.MultipartBodyLengthLimit = 1024L * 1024 * 1024 * 5; // 5 GB
    options.MultipartHeadersLengthLimit = 1024 * 1024 * 1024; // 1 GB
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1024L * 1024 * 1024 * 5; // 5 GB
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var lineArgs = Environment.GetCommandLineArgs();
    if (!lineArgs.Any(p => p.Contains("NSwag.AspNetCore.Launcher.dll")))
    {
        await app.InitialiseDatabaseAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});

app.AddLocalization();

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.UseExceptionHandler(options => { });


app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowAnyOrigin();
});


app.MapEndpoints();

app.Run();


public partial class Program { }
