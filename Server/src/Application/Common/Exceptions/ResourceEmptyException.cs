namespace backend.Application.Common.Exceptions;

public class ResourceEmptyException : Exception
{
    public ResourceEmptyException() : base("Error while saving resource to disk, resource content empty.") { }
}