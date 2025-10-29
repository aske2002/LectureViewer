using System.Net.Http.Json;
using System.Net.Mime;
using AutoMapper;
using backend.Application.Common.Interfaces;
using backend.Application.Countries.Queries.GetCountries;
using backend.Domain.Entities;
using backend.Domain.Enums;
using HeyRed.Mime;
using Microsoft.EntityFrameworkCore;

namespace backend.Infrastructure.Data.Seeders;
public class CountriesDataSeeder : IDataSeeder
{
    public record CountryHrefResponse(string flag);
    public record CountryResponse(string name, List<string> tld, string iso2, string iso3, CountryHrefResponse href);
    public record CountryResponseList(List<CountryResponse> data);
    private readonly IApplicationDbContext _context;
    private readonly IResourceService _resourceService;
    private readonly HttpClient _httpClient;

    public CountriesDataSeeder(IApplicationDbContext context, IResourceService resourceService, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient("RestfulCountries");
        _resourceService = resourceService;
    }

    private async Task<(CountryResponse country, byte[]? flagBytes)[]> GetAllCountriesAsync()
    {
        var response = await _httpClient.GetAsync("countries");
        response.EnsureSuccessStatusCode();

        CountryResponseList countries = await response.Content.ReadFromJsonAsync<CountryResponseList>()
            ?? throw new Exception("Failed to deserialize countries data.");

        // Start downloading all flags in parallel
        var flagDownloadTasks = countries.data.Select(async country =>
        {
            var flagUrl = country.href.flag;

            try
            {
                var flagResponse = await _httpClient.GetAsync(flagUrl);
                flagResponse.EnsureSuccessStatusCode();

                var flagBytes = await flagResponse.Content.ReadAsByteArrayAsync();

                // You can store the bytes in the country object if you want
                // e.g., country.FlagBytes = flagBytes;
                return (country, flagBytes);
            }
            catch
            {
                return (country, (byte[]?)null); // Return empty byte array on failure
            }
        });
        var countriesWithFlags = await Task.WhenAll(flagDownloadTasks);
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
            Name = countryResponse.country.name,
            IsoCode = countryResponse.country.iso2,
            Description = countryResponse.country.name,
        }).ToList();

        await _context.Countries.AddRangeAsync(countryEntities, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        foreach (var countryResponse in countryResponses)
        {
            var country = countryEntities.FirstOrDefault(c => c.IsoCode == countryResponse.country.iso2);
            if (country == null || countryResponse.flagBytes == null)
            {
                continue;
            }

            var flagUrl = new Uri(countryResponse.country.href.flag);
            var flagFileName = Path.GetFileName(flagUrl.LocalPath);
            var mimeType = MimeTypesMap.GetMimeType(flagFileName);

            var resource = await _resourceService.CreateResourceAsync(
                country.Id.Value,
                flagFileName,
                ResourceType.Flag,
                countryResponse.flagBytes.Length,
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