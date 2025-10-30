using backend.Application.Common.Security;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Semesters.Queries.GetSemesterById;

[Authorize]
public record GetSemesterQuery(SemesterId Id) : IRequest<SemesterDto>;

public class GetSemesterQueryHandler : IRequestHandler<GetSemesterQuery, SemesterDto>
{
    private readonly IRepository<Semester, SemesterId> _repository;
    private readonly IMapper _mapper;

    public GetSemesterQueryHandler(IRepository<Semester, SemesterId> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SemesterDto> Handle(GetSemesterQuery request, CancellationToken cancellationToken)
    {
        var response = await _repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        return _mapper.Map<SemesterDto>(response);
    }
}
