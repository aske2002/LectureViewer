using backend.Application.Common.Interfaces;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.TodoLists.Commands.CreateTodoList;

public record CreateTodoListCommand : IRequest<TodoListId>
{
    public string? Title { get; init; }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, TodoListId>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoListId> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList();

        entity.Title = request.Title;

        _context.TodoLists.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
