using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;

namespace backend.Domain.Helpers;

public class StronglyTypedIdConverter<TId> : TypeConverter
where TId : StronglyTypedId<TId>
{
    private static readonly TypeConverter IdValueConverter = GetIdValueConverter();

    private static TypeConverter GetIdValueConverter()
    {
        return TypeDescriptor.GetConverter(typeof(Guid));
    }

    private readonly Type _stronglyTypedIdType;
    public StronglyTypedIdConverter(Type stronglyTypedIdType)
    {
        if (!StronglyTypedIdHelper.IsStronglyTypedId(stronglyTypedIdType))
            throw new ArgumentException($"Type '{stronglyTypedIdType}' is not a strongly-typed id type", nameof(stronglyTypedIdType));
        _stronglyTypedIdType = stronglyTypedIdType;
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string)
            || sourceType == typeof(Guid)
            || base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string)
            || destinationType == typeof(Guid)
            || base.CanConvertTo(context, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string s)
        {
            value = IdValueConverter.ConvertFrom(s)
                    ?? throw new InvalidOperationException($"Cannot convert '{s}' to Guid");
        }

        if (value is Guid idValue)
        {
            var factory = StronglyTypedIdHelper.GetFactory(_stronglyTypedIdType);
            return factory(idValue);
        }

        return base.ConvertFrom(context, culture, value)
               ?? throw new InvalidOperationException(
                   $"Type '{_stronglyTypedIdType}' doesn't have a constructor that can convert from '{value?.GetType()}'");
    }

    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        var stronglyTypedId = (StronglyTypedId<TId>)value;
        Guid idValue = stronglyTypedId.Value;
        if (destinationType == typeof(string))
            return idValue.ToString()!;
        if (destinationType == typeof(Guid))
            return idValue;
        return base.ConvertTo(context, culture, value, destinationType)
               ?? throw new InvalidOperationException(
                     $"Type '{_stronglyTypedIdType}' doesn't have a constructor that can convert from '{value?.GetType()}'");
    }
}

public class StronglyTypedIdConverter : TypeConverter
{
    private static readonly ConcurrentDictionary<Type, TypeConverter> ActualConverters = new();

    private readonly TypeConverter _innerConverter;

    public StronglyTypedIdConverter(Type stronglyTypedIdType)
    {
        _innerConverter = ActualConverters.GetOrAdd(stronglyTypedIdType, CreateActualConverter);
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        _innerConverter.CanConvertFrom(context, sourceType);
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
        _innerConverter.CanConvertTo(context, destinationType);
    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        _innerConverter.ConvertFrom(context, culture, value)
               ?? throw new InvalidOperationException(
                   $"Type '{_innerConverter.GetType()}' doesn't have a constructor that can convert from '{value?.GetType()}'");
    public override object ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) =>
        _innerConverter.ConvertTo(context, culture, value, destinationType)
               ?? throw new InvalidOperationException(
                   $"Type '{_innerConverter.GetType()}' doesn't have a constructor that can convert from '{value?.GetType()}'");


    private static TypeConverter CreateActualConverter(Type stronglyTypedIdType)
    {
        if (!StronglyTypedIdHelper.IsStronglyTypedId(stronglyTypedIdType, out var idType))
            throw new InvalidOperationException($"The type '{stronglyTypedIdType}' is not a strongly typed id");

        var actualConverterType = typeof(StronglyTypedIdConverter<>).MakeGenericType(idType);
        return (TypeConverter)Activator.CreateInstance(actualConverterType, stronglyTypedIdType)!;
    }
}