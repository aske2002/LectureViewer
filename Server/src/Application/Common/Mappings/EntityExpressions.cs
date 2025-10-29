using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Domain.Extensions;

namespace backend.Application.Common.Mappings;

public record EntityTypeData<T>(Func<T, ResourceResponse?> resourceAccessor,
        Action<T, ResourceResponse?> resourceSetter,
        string fullPath,
        PropertyInfo type,
        Func<T, Guid> idGetter);

public static class ResourceAccessorCollector
{
    public static List<EntityTypeData<T>> CollectAllResourceAccessorsAlongPath<T>(string? path)
    {
        List<EntityTypeData<T>> entityTypeDatas = new();

        var pathParts = string.IsNullOrEmpty(path)
            ? Array.Empty<string>()
            : path.Split('.');

        Type currentType = typeof(T);
        var currentPath = string.Empty;

        foreach (var part in pathParts)
        {
            CollectRecursive<T>(currentType, entityTypeDatas, currentPath);

            currentType = currentType.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                ?.PropertyType ?? throw new InvalidOperationException($"Property '{part}' not found on {currentType}");
            currentPath = string.IsNullOrEmpty(currentPath) ? part : $"{currentPath}.{part}";
        }
        CollectRecursive<T>(currentType, entityTypeDatas, currentPath);
        return entityTypeDatas;
    }

    private static void CollectRecursive<T>(Type type, List<EntityTypeData<T>> data, string pathPrefix)
    {
        if (!type.IsBaseResponse())
            return;

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var fullPath = string.IsNullOrEmpty(pathPrefix) ? prop.Name : $"{pathPrefix}.{prop.Name}";
            var propType = prop.PropertyType;

            if (typeof(ResourceResponse).IsAssignableFrom(propType) || IsEnumerableOfResourceResponse(propType))
            {
                var baseResponseIdPath = string.IsNullOrEmpty(pathPrefix) ? $"Id.Value" : $"{pathPrefix}.Id.Value";
                var accessor = CreateAccessor<T, ResourceResponse>(fullPath);
                var setter = CreateSetter<T>(fullPath);
                var baseResponseAccessor = CreateAccessor<T, Guid>(baseResponseIdPath);
                var entityTypeData = new EntityTypeData<T>(accessor, setter, fullPath, prop, baseResponseAccessor);
                data.Add(entityTypeData);
            }
        }
    }

    private static Func<T, V> CreateAccessor<T, V>(string fullPath)
    {
        var param = Expression.Parameter(typeof(T), "obj");
        Expression body = param;

        foreach (var part in fullPath.Split('.'))
        {
            var prop = body.Type.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"Property '{part}' not found on {body.Type}");

            body = Expression.Property(body, prop);

        }

        var convert = Expression.Convert(body, typeof(V));
        var lambda = Expression.Lambda<Func<T, V>>(convert, param);
        return lambda.Compile();
    }

    public static Action<T, ResourceResponse?> CreateSetter<T>(string propertyPath)
    {
        var paramObj = Expression.Parameter(typeof(T), "obj");
        var paramValue = Expression.Parameter(typeof(ResourceResponse), "value");

        Expression current = paramObj;
        foreach (var part in propertyPath.Split('.'))
        {
            var prop = current.Type.GetProperty(part, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"Property '{part}' not found on {current.Type}");
            current = Expression.Property(current, prop);
        }

        var body = Expression.Assign(current, Expression.Convert(paramValue, current.Type));
        var lambda = Expression.Lambda<Action<T, ResourceResponse?>>(body, paramObj, paramValue);
        return lambda.Compile();
    }


    private static bool IsEnumerableOfResourceResponse(Type type)
    {
        if (type == typeof(string)) return false;
        if (!typeof(IEnumerable).IsAssignableFrom(type)) return false;

        var elementType = GetEnumerableElementType(type);
        return elementType != null && typeof(ResourceResponse).IsAssignableFrom(elementType);
    }

    private static Type? GetEnumerableElementType(Type type)
    {
        if (type.IsArray) return type.GetElementType();

        if (type.IsGenericType)
        {
            return type.GetGenericArguments()[0];
        }

        var iface = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        return iface?.GetGenericArguments()[0];
    }
}
