namespace backend.Application.Common.Exceptions;

public class ResourceAlreadyExistsException : Exception
{
    public ResourceAlreadyExistsException() : base() { }
    public ResourceAlreadyExistsException(string message) : base(message) { }
    public ResourceAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
}