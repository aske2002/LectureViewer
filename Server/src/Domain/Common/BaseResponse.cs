
public interface IBaseResponse
{
    public IStronglyTypedId Id { get; init; }
}

public abstract record BaseResponse<T> where T : StronglyTypedId<T>
{
    public T Id { get; init; } = StronglyTypedId<T>.Default();
}