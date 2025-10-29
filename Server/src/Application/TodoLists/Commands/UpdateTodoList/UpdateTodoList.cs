using backend.Application.Common.Interfaces;
using backend.Domain.Common;
using backend.Domain.Identifiers;

namespace backend.Application.TodoLists.Commands.UpdateTodoList;

public record UpdateTodoListCommand : IRequest
{
    public TodoListId Id { get; init; } = TodoListId.Default();

    public string? Title { get; init; }
}

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id.Value, entity);

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);

    }
}
