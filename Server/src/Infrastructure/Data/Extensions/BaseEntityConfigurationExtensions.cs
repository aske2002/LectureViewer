using System.Diagnostics;
using System.Reflection;
using backend.Domain.Common;
using backend.Domain.Extensions;
using backend.Domain.Helpers;
using backend.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace backend.Infrastructure.Data.Extensions;

public static class BaseEntityConfigurationExtensions
{
    public static void ConfigureEntities(this ModelBuilder builder)
    {

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            Console.WriteLine($"Entity base type: {entityType.ClrType.Name} => {entityType.BaseType?.Name}");
            if (entityType.BaseType != null)
                continue;

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
                        configureMethod.Invoke(configurationInstance, [entityTypeBuilder]);
                    }
                }
            }
        }

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                var clr = property.ClrType;

                if (!StronglyTypedIdHelper.IsStronglyTypedId(clr)) continue;

                // Create converter and comparer generically
                var converterType = typeof(StronglyTypedIdValueConverter<>).MakeGenericType(clr);
                var comparerType = typeof(StronglyTypedIdValueComparer<>).MakeGenericType(clr);

                var converter = (ValueConverter)Activator.CreateInstance(converterType)!;
                var comparer = (ValueComparer)Activator.CreateInstance(comparerType)!;

                property.SetValueConverter(converter);
                property.SetValueComparer(comparer);
            }
        }

    }
}


public class StronglyTypedIdValueConverter<TId> : ValueConverter<TId, Guid>
    where TId : IStronglyTypedId
{
    public StronglyTypedIdValueConverter()
        : base(
            id => id.Value,
            value => (TId)Activator.CreateInstance(typeof(TId), value)!)
    { }
}
public class StronglyTypedIdValueComparer<TId> : ValueComparer<TId>
    where TId : IStronglyTypedId
{
    public StronglyTypedIdValueComparer()
        : base(
            (a, b) => a != null && b != null && a.Value == b.Value,
            a => a.Value.GetHashCode(),
            a => (TId)Activator.CreateInstance(typeof(TId), a.Value)!)
    { }
}