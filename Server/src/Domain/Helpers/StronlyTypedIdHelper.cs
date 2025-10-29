using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace backend.Domain.Helpers;

public static class StronglyTypedIdHelper
{
    private static readonly ConcurrentDictionary<Type, Delegate> StronglyTypedIdFactories = new();
    public static Func<Guid, object> GetFactory(Type stronglyTypedIdType)
    {
        return (Func<Guid, object>)StronglyTypedIdFactories.GetOrAdd(
            stronglyTypedIdType,
            CreateFactory);
    }

    private static Func<Guid, object> CreateFactory(Type stronglyTypedIdType)
    {
        if (!IsStronglyTypedId(stronglyTypedIdType))
            throw new ArgumentException($"Type '{stronglyTypedIdType}' is not a strongly-typed id type", nameof(stronglyTypedIdType));

        var ctor = stronglyTypedIdType.GetConstructor(new[] { typeof(Guid) });
        if (ctor is null)
            throw new InvalidOperationException($"Type '{stronglyTypedIdType}' doesn't have a constructor that takes a Guid");

        var param = Expression.Parameter(typeof(Guid), "value");
        var body = Expression.New(ctor, param);
        var lambda = Expression.Lambda<Func<Guid, object>>(body, param);
        return lambda.Compile();
    }

    public static bool IsStronglyTypedId(Type type) => IsStronglyTypedId(type, out _);


    public static bool IsStronglyTypedId(Type type, [NotNullWhen(true)] out Type? idType)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        if (type.BaseType is Type baseType &&
            baseType.IsGenericType &&
            baseType.GetGenericTypeDefinition() == typeof(StronglyTypedId<>))
        {
            idType = baseType.GenericTypeArguments[0];
            return true;
        }


        idType = null;
        return false;
    }
}