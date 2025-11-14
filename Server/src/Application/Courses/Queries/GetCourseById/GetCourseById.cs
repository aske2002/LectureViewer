using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Courses.Queries.GetCourseById;

[Authorize]
public record GetCourseQuery(CourseId Id) : IRequest<CourseDetailsDto>;

public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, CourseDetailsDto>
{
    private readonly ICourseService _service;
    private readonly IMapper _mapper;

    public GetCourseQueryHandler(ICourseService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<CourseDetailsDto> Handle(GetCourseQuery request, CancellationToken cancellationToken)
    {
        var response = await _service.GetCourseDetailsAsync(request.Id);
        return _mapper.Map<CourseDetailsDto>(response);
    }
}
