namespace backend.Domain.Identifiers;

public class TodoItemId : StronglyTypedId<TodoItemId>
{
    public TodoItemId(Guid value) : base(value) {}
}