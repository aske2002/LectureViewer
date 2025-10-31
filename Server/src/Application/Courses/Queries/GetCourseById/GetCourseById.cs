using backend.Application.Common.Security;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Courses.Queries.GetCourseById;

[Authorize]
public record GetCourseQuery(CourseId Id) : IRequest<CourseEntityDto>;

public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, CourseEntityDto>
{
    private readonly IRepository<Course, CourseId> _repository;
    private readonly IMapper _mapper;

    public GetCourseQueryHandler(IRepository<Course, CourseId> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CourseEntityDto> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        var response = await _repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);
        return _mapper.Map<CourseEntityDto>(response);
    }
}
