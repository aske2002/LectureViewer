using System.ComponentModel;
using System.Reflection;
using backend.Domain.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace backend.Domain.Helpers;

public static class StronglyTypedIdConverterRegistration
{
    public static void RegisterStronglyTypedIdConverters(this IServiceCollection serviceProvider)
    {
        var assemblies = new[] { Assembly.GetExecutingAssembly() };

        var stronglyTypedIdBaseType = typeof(StronglyTypedId<>);
        var converterBaseType = typeof(StronglyTypedIdConverter<>);

        var stronglyTypedIdTypes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                !t.IsAbstract &&
                stronglyTypedIdBaseType.IsAssignableFrom(t) &&
                t.GetConstructor(new[] { typeof(Guid) }) is not null
            );

        foreach (var idType in stronglyTypedIdTypes)
        {
            var converter = converterBaseType.MakeGenericType(idType);
            TypeDescriptor.AddAttributes(idType, new TypeConverterAttribute(converter));
        }
    }
}
