using System.Reflection;

public interface INotFoundException
{
    string EntityName { get; }
    string? ErrorDescription { get; set; }
    string Id { get; }
    string Message { get; }
}

public abstract class NotFoundExceptionBase<IdType, TEntity> : Exception, INotFoundException
    where IdType : notnull
    where TEntity : NotFoundExceptionBase<IdType, TEntity>
{
    public abstract string EntityName { get; }
    public string? ErrorDescription { get; set; }
    public string Id => id.ToString() ?? string.Empty;
    private IdType id { get; }

    public static TEntity Create(IdType id, string? errorDescription = null)
    {
        var exception = (TEntity?)Activator.CreateInstance(typeof(TEntity), id);
        if (exception == null)
        {
            throw new InvalidOperationException($"Cannot create instance of {typeof(TEntity)}, " +
                $"make sure it has a constructor that takes {typeof(IdType).Name}.");
        }
        exception.ErrorDescription = errorDescription;
        return exception;
    }
    public override string Message
    {
        get
        {
            var message = $"The {EntityName} with ID {id} was not found.";
            if (!string.IsNullOrEmpty(ErrorDescription))
            {
                message += $" {ErrorDescription}";
            }
            return message;
        }
    }
    public NotFoundExceptionBase(IdType Id) : base($"Resource with ID {Id} not found.")
    {
        id = Id;
    }

    public NotFoundExceptionBase(IdType Id, string message) : this(Id)
    {
        id = Id;
        ErrorDescription = message;
    }
}

public static class NotFoundExceptionBase
{
    public static ICollection<Type> GetNotFoundExceptionTypes()
    {
        return typeof(NotFoundExceptionBase<,>).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.BaseType != null && t.BaseType.IsGenericType && t.BaseType.GetGenericTypeDefinition() == typeof(NotFoundExceptionBase<,>))
            .ToList();
    }
}