using backend.Domain.Common;
using backend.Domain.Extensions;
using backend.Domain.Interfaces;
using backend.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Infrastructure.Data.Extensions;
public static class BaseEntityConfigurationExtensions
{
    public static void ConfigureBaseEntity(this ModelBuilder builder)
    {

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.IsBaseAuditableEntity(out _, out var idType))
            {
                var configurationType = typeof(BaseConfiguration<,>).MakeGenericType(entityType.ClrType, idType);
                var configurationInstance = Activator.CreateInstance(configurationType);
                var configureMethod = configurationType.GetMethod("Configure");
                if (configureMethod != null)
                {
                    var entityTypeBuilder = typeof(ModelBuilder).GetMethods()
                        .FirstOrDefault(m => m.Name == "Entity" && m.IsGenericMethod && m.GetParameters().Length == 0)
                        ?.MakeGenericMethod(entityType.ClrType)
                        ?.Invoke(builder, null);

                    if (entityTypeBuilder != null)
                    {
                        configureMethod.Invoke(configurationInstance, new object[] { entityTypeBuilder });
                    }
                }
            }
        }
    }
}