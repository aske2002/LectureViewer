
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using backend.Domain.Helpers;
using backend.Domain.Interfaces;

namespace backend.Domain.Extensions;
public static class EntityTypeExtensions
{
    public record EntityTypeInfo(Type DomainType, Type BaseType, List<Type> DomainGenericParamsOnBase);
    private static readonly List<Type> checkedTypes = new() { };
    private static readonly ImmutableList<Type> typesToCheck = ImmutableList<Type>.Empty.AddRange(
    [
        typeof(BaseEntity<>),
        typeof(BaseAuditableEntity<>),
        typeof(StronglyTypedId<>),
        typeof(IRepository<,>)
    ]);

    public static List<Assembly> GetAllAssemblies()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var assemblyName = executingAssembly.GetName().Name?.Split('.').FirstOrDefault();
        if (assemblyName == null)
            throw new InvalidOperationException("Assembly name is null");

        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.FullName?.StartsWith(assemblyName) == true)
            .ToList();
        return assemblies;
    }

    private static (List<Type> AssemblyTyped, ConcurrentDictionary<(Type baseType, Type checkedType), (Type entityType, List<Type> genericTypes)?> DerviedFromCache)? _cache;

    public static List<EntityTypeInfo> GetAllDerivedTypes(this Type baseType)
    {
        if (!checkedTypes.Contains(baseType))
            CheckAllTypesExtending(baseType);

        return cache.DerviedFromCache.Where(kvp => kvp.Key.baseType == baseType && kvp.Value != null)
            .Select(kvp => new EntityTypeInfo(kvp.Key.checkedType, kvp.Value!.Value.entityType, kvp.Value!.Value.genericTypes))
            .ToList();
    }

    public static List<EntityTypeInfo> WhereDerivesFrom(this IEnumerable<Type> types, Type baseType)
    {
        List<EntityTypeInfo> result = new();
        foreach (var type in types)
        {
            if (type.IsDerivedFrom(baseType, out var entityType, out var generics))
            {
                if (entityType != null && generics != null)
                {
                    var entityTypeInfo = new EntityTypeInfo(type, entityType, generics);
                    result.Add(entityTypeInfo);
                }
            }
        }
        return result;
    }

    private static List<Type> CheckAllTypesExtending(Type type)
    {
        var assemblyTypes = cache.AssemblyTypes;
        foreach (var assemblyType in assemblyTypes)
        {
            assemblyType.IsDerivedFrom(type);
        }
        checkedTypes.Add(type);
        return new List<Type>();
    }



    public static void Initialize()
    {
        foreach (var type in typesToCheck)
        {
            CheckAllTypesExtending(type);
        }
    }

    public static bool IsBaseResponse(this Type type)
    {
        return type.IsDerivedFrom(typeof(BaseResponse<>));
    }

    /// <summary>
    /// Checks if the given type is derived from BaseEntity.
    /// </summary>
    public static bool IsBaseEntity(this Type typeToCheck)
    {
        return IsBaseEntity(typeToCheck, out _, out _);
    }

    /// <summary>
    /// Checks if the given type is derived from BaseEntity and returns the entity type and id type.
    /// </summary>
    /// <param name="typeToCheck">The type to check.</param>
    /// <param name="entityType">The entity type if derived from BaseEntity, otherwise null.</param>
    /// <param name="idType">The id type if derived from BaseEntity, otherwise null.</param>
    /// <returns>True if the type is derived from BaseEntity, otherwise false.</returns>
    /// </summary>
    public static bool IsBaseEntity(this Type typeToCheck, [NotNullWhen(true)] out Type? entityType, [NotNullWhen(true)] out Type? idType)
    {
        idType = null;
        if (IsDerivedFrom(typeToCheck, typeof(BaseEntity<>), out entityType, out var generics))
        {
            idType = generics?.FirstOrDefault();
            if (idType != null && entityType != null)
                return true;
        }
        return false;
    }
    public static bool IsBaseAuditableEntity(this Type typeToCheck)
    {
        return IsBaseAuditableEntity(typeToCheck, out _, out _);
    }

    public static bool IsBaseAuditableEntity(this Type typeToCheck, [NotNullWhen(true)] out Type? entityType, [NotNullWhen(true)] out Type? idType)
    {
        idType = null;
        if (IsDerivedFrom(typeToCheck, typeof(BaseAuditableEntity<>), out entityType, out var generics))
        {
            idType = generics?.FirstOrDefault();
            if (idType != null && entityType != null)
                return true;
        }
        return false;
    }

    public static bool IsDerivedFrom(this Type typeToCheck, Type baseType)
    {
        return IsDerivedFrom(typeToCheck, baseType, out _, out _);
    }

    private static (List<Type> AssemblyTypes, ConcurrentDictionary<(Type baseType, Type checkedType), (Type entityType, List<Type> genericTypes)?> DerviedFromCache) cache
    {
        get
        {
            if (_cache == null)
            {
                var types = GetAllAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition)
                    .ToList();
                _cache = (types, new ConcurrentDictionary<(Type baseType, Type checkedType), (Type entityType, List<Type> genericTypes)?>());
            }
            return _cache.Value;
        }
    }

    public static bool IsDerivedFrom(this Type typeToCheck, Type baseType, [NotNullWhen(true)] out Type? entityType, [NotNullWhen(true)] out List<Type>? generics)
    {

        (entityType, generics) = (null, null);
        if (cache.DerviedFromCache.TryGetValue((baseType, typeToCheck), out var cachedValue))
        {
            if (cachedValue != null)
            {
                entityType = cachedValue.Value.entityType;
                generics = cachedValue.Value.genericTypes;
                return true;
            }
            else
            {
                return false;
            }
        }

        if (baseType.IsInterface)
        {
            var matchingInterface = typeToCheck.GetInterfaces()
                .FirstOrDefault(i => (i.IsGenericType && i.GetGenericTypeDefinition() == baseType) || i == baseType);

            if (matchingInterface != null)
            {
                cache.DerviedFromCache.TryAdd((baseType, typeToCheck), (matchingInterface, matchingInterface.GenericTypeArguments.ToList()));
                entityType = matchingInterface;
                generics = matchingInterface.GenericTypeArguments.ToList();
                return true;
            }
            else
            {
                cache.DerviedFromCache.TryAdd((baseType, typeToCheck), null);
                return false;
            }

        }


        var type = typeToCheck.BaseType;
        while (type != null && type != typeof(object))
        {
            if (baseType.IsGenericType && type.IsGenericType && type.GetGenericTypeDefinition() == baseType || baseType == type)
            {
                cache.DerviedFromCache.TryAdd((baseType, typeToCheck), (type, type.GenericTypeArguments.ToList()));
                entityType = type;
                generics = type.GenericTypeArguments.ToList();
                return true;
            }
            type = type.BaseType;
        }

        cache.DerviedFromCache.TryAdd((baseType, typeToCheck), null);
        return false;
    }
}