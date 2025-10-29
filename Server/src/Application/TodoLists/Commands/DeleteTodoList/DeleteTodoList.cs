using backend.Application.Common.Interfaces;
using backend.Domain.Identifiers;

namespace backend.Application.TodoLists.Commands.DeleteTodoList;

public record DeleteTodoListCommand(TodoListId Id) : IRequest;

public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.Id.Value, entity);

        _context.TodoLists.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
