using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Events;
using backend.Domain.Identifiers;

namespace backend.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : IRequest<TodoItemId>
{
    public TodoListId ListId { get; init; } = TodoListId.Default();

    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItemId>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItemId> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        _context.TodoItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
