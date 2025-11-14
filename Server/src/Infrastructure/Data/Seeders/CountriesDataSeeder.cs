using System.Text.Json.Serialization;
using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Enums;
using HeyRed.Mime;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data.Seeders;
public class CountriesDataSeeder : IDataSeeder
{
    public record CountryHrefResponse(string flag);
    public record CountryResponse(string en, string alpha2, string alpha3,
    string ar, string bg, string br, string cs, string da, string de, string el, string eo, string es, string et, string eu, string fa, string fi, string fr, string hr, string hu, string hy, string it, string ja, string ko, string lt, string nl, string no, string pl, string pt, string ro, string ru, string sk, string sl, string sr, string sv, string th, string tr, string uk, string zh, [property:JsonPropertyName("zh-tw")] string zhtw);
    private readonly IApplicationDbContext _context;
    private readonly IResourceService _resourceService;

    public CountriesDataSeeder(IApplicationDbContext context, IResourceService resourceService)
    {
        _context = context;
        _resourceService = resourceService;
    }

    private async Task<(CountryResponse country, byte[]? flagBytes, string flagPath)[]> GetAllCountriesAsync()
    {


        var path = Path.Combine(AppContext.BaseDirectory, "Resources", "Countries", "all.json");
        using var stream = File.OpenRead(path);

        var countries = await System.Text.Json.JsonSerializer.DeserializeAsync<List<CountryResponse>>(stream) ?? throw new Exception("Failed to deserialize countries data.");

        // Start reading all flags in parallel
        var flagReadTasks = countries.Select(async country =>
        {
           var flagPath = Path.Combine(AppContext.BaseDirectory, "Resources", "Countries", "Flags", $"{country.alpha2.ToLower()}.png");

            try
            {
                var flagBytes = await File.ReadAllBytesAsync(flagPath);
                return (country, flagBytes, flagPath);
            }
            catch
            {
                return (country, (byte[]?)null, flagPath);
            }
        });
        var countriesWithFlags = await Task.WhenAll(flagReadTasks);
        return countriesWithFlags;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var localCountries = await _context.Countries.Select(c => c.IsoCode).ToListAsync(cancellationToken);
        var countryResponses = await GetAllCountriesAsync();

        if (countryResponses == null)
        {
            throw new Exception("Failed to fetch countries data.");
        }

        var countryEntities = countryResponses.Select(countryResponse => new Country
        {
            Name = countryResponse.country.en,
            IsoCode = countryResponse.country.alpha2,
            Description = countryResponse.country.en,
        }).ToList();

        await _context.Countries.AddRangeAsync(countryEntities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var countryResponse in countryResponses)
        {
            var country = countryEntities.FirstOrDefault(c => c.IsoCode == countryResponse.country.alpha2);
            if (country == null || countryResponse.flagBytes == null)
            {
                continue;
            }

            var flagUrl = new Uri(countryResponse.flagPath);
            var flagFileName = Path.GetFileName(flagUrl.LocalPath);
            var mimeType = MimeTypesMap.GetMimeType(flagFileName);

            var resource = await _resourceService.CreateResourceAsync(
                flagFileName,
                ResourceType.Flag,
                countryResponse.flagBytes,
                mimeType ?? "image/png",
                null,
                cancellationToken: cancellationToken
            );
        }
    }

    public async Task<bool> ShouldSeedAsync()
    {
        return await _context.Countries.CountAsync() == 0;
    }
}