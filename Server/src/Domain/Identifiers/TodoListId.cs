using System.ComponentModel;
using backend.Domain.Helpers;

namespace backend.Domain.Identifiers;

public class TodoListId : StronglyTypedId<TodoListId>
{
    public TodoListId(Guid value) : base(value) {}
}