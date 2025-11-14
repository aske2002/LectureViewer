using System.ComponentModel;
using System.Net.Http;
using System.Reflection;
using backend.Domain.Helpers;

namespace backend.Domain.Common;

public interface IStronglyTypedId
{
    Guid Value { get; }
}

[TypeConverter(typeof(StronglyTypedIdConverter))]
public abstract class StronglyTypedId<T> : IStronglyTypedId, IEquatable<StronglyTypedId<T>>
 where T : StronglyTypedId<T>
{
    public Guid Value { get; }

    protected StronglyTypedId(Guid value) => Value = value;

    public override string ToString() => Value.ToString();
    public static T Default()
    {
        var factory = StronglyTypedIdHelper.GetFactory(typeof(T));
        return (T)factory(default);
    }

    public static bool TryParse(string? value, IFormatProvider? provider, out T id)
    {
        if (Guid.TryParse(value, out var guid))
        {
            var factory = StronglyTypedIdHelper.GetFactory(typeof(T));
            id = (T)factory(guid);
            return true;
        }

        id = Default();
        return false;
    }

    // Equality members
    public bool Equals(StronglyTypedId<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;

        return obj is StronglyTypedId<T> other && Equals(other);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(StronglyTypedId<T>? left, StronglyTypedId<T>? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Value == right.Value;
    }

    public static bool operator !=(StronglyTypedId<T>? left, StronglyTypedId<T>? right)
        => !(left == right);
}