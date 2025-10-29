using backend.Domain.Extensions;
using backend.Infrastructure.Data.Seeders;
using Microsoft.Extensions.DependencyInjection;

public static class IServiceProviderExtensions
{
    public static IServiceCollection ConfigureDataSeeders(this IServiceCollection serviceProvider)
    {
        var dataSeeders = typeof(IDataSeeder).GetAllDerivedTypes();
        foreach (var dataSeeder in dataSeeders)
        {
            serviceProvider.AddScoped(typeof(IDataSeeder), dataSeeder.DomainType);  
        }
        return serviceProvider;
    }
}