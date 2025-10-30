using backend.Application.Common.Interfaces;
using backend.Application.Common.Models;
using backend.Application.Semesters.Queries.GetSemesterById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Semesters.Queries.ListSemesters;

public record ListSemestersWithPaginationQuery : IRequest<FilteredList<SemesterDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class ListSemestersWithPaginationQueryHandler : IRequestHandler<ListSemestersWithPaginationQuery, FilteredList<SemesterDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IRepository<Semester, SemesterId> _repository;

    public ListSemestersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, IRepository<Semester, SemesterId> repository)
    {
        _context = context;
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<FilteredList<SemesterDto>> Handle(ListSemestersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var semesters = await _repository.QueryAsync<SemesterDto>(
            orderBy: q => q.OrderBy(s => s.Year).ThenBy(s => s.Season),
            cancellationToken: cancellationToken,
            pagination: (request.PageNumber, request.PageSize));
        return FilteredList<SemesterDto>.Create(semesters, orderBy: null);
    }
}
