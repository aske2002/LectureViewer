using backend.Domain.Extensions;
using backend.Infrastructure.Data;
using StackExchange.Profiling.Storage;


EntityTypeExtensions.Initialize();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();

builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";          // URL where you can view results
    if (options.Storage is MemoryCacheStorage storage)
    {
        storage.CacheDuration = TimeSpan.FromMinutes(60);
    }
    // 👇 Optional: limit to last 100 requests in memory
    options.ResultsAuthorize = _ => true;   // allow anyone to view results
    options.ResultsListAuthorize = _ => true;

    options.TrackConnectionOpenClose = true;      // show EF connection timings
}).AddEntityFramework();
builder.AddInfrastructureServices();
builder.AddWebServices();

var app = builder.Build();

app.UseMiniProfiler();

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
