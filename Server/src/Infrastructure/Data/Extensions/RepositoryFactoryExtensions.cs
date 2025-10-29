using backend.Domain.Common;
using backend.Domain.Extensions;
using backend.Domain.Interfaces;
using backend.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Infrastructure.Data.Extensions;
public static class RepositoryFactoryExtensions
{



    public static IServiceCollection ConfigureRepositories(this IServiceCollection provider)
    {
        Console.WriteLine("Registering repositories");
        var baseEntityTypes = typeof(BaseEntity<>).GetAllDerivedTypes();
        var existingRepositories = typeof(IRepository<,>).GetAllDerivedTypes();

        var missingEntitityRepositories = baseEntityTypes
            .Where(entityType => !existingRepositories.Any(r => r.DomainGenericParamsOnBase[0] == entityType?.DomainType &&
                                r.DomainGenericParamsOnBase[1] == entityType.DomainGenericParamsOnBase[0]));


        foreach (var entityType in missingEntitityRepositories)
        {
            Console.WriteLine($"Registering repository for {entityType.DomainType.Name}");
            var idType = entityType.DomainGenericParamsOnBase[0];
            var repositoryInterface = typeof(IRepository<,>).MakeGenericType(entityType.DomainType, idType);
            var repositoryType = typeof(DefaultRepositoryImplementation<,>).MakeGenericType(entityType.DomainType, idType);
            provider = provider.AddScoped(repositoryInterface, repositoryType);
        }
        return provider;

    }
}