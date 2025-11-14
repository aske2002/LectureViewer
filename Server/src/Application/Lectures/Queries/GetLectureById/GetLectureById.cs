using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Application.Courses.Queries.GetCourseById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Lectures.Queries.GetLectureById;

[Authorize]
public record GetLectureQuery(CourseId CourseId, LectureId LectureId) : IRequest<LectureDto>;

public class GetLectureQueryHandler : IRequestHandler<GetLectureQuery, LectureDto>
{
    private readonly ICourseService _service;
    private readonly IMapper _mapper;

    public GetLectureQueryHandler(ICourseService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<LectureDto> Handle(GetLectureQuery request, CancellationToken cancellationToken)
    {
        var response = await _service.GetLectureDetailsAsync(request.CourseId, request.LectureId);
        return _mapper.Map<LectureDto>(response);
    }
}
