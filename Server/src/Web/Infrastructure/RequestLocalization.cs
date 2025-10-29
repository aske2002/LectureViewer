using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace backend.Web.Infrastructure;
public static class RequestLocalization
{
    private static readonly string[] SupportedCultures = { "en-GB", "da-DK" };
    public static IApplicationBuilder AddLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = SupportedCultures.Select(c => new CultureInfo(c)).ToList();
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(supportedCultures.First()),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            ApplyCurrentCultureToResponseHeaders = true,
            RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new AcceptLanguageHeaderRequestCultureProvider(),
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
            }
        };

        app.UseRequestLocalization(localizationOptions);
        return app;
    }
}