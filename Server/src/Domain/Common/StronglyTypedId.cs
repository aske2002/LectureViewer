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
public abstract class StronglyTypedId<T> : IStronglyTypedId where T : StronglyTypedId<T>
{
    public Guid Value { get; }
    
    protected StronglyTypedId(Guid value) => Value = value;

    public override string ToString() => Value.ToString();
    public static T Default()
    {
        var factory = StronglyTypedIdHelper.GetFactory(typeof(T));
        return (T)factory(Guid.Empty);
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
}