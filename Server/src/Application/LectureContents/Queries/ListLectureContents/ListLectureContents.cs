using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Application.Courses.Queries.GetCourseById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.LectureContents.Queries.ListLectureContents;

[Authorize]
public record ListLectureContentsQuery(CourseId CourseId, LectureId LectureId) : IRequest<LectureContentListDto>;

public class ListLectureContentsHandler : IRequestHandler<ListLectureContentsQuery, LectureContentListDto>
{
    private readonly ICourseService _service;
    private readonly IMapper _mapper;

    public ListLectureContentsHandler(ICourseService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<LectureContentListDto> Handle(ListLectureContentsQuery request, CancellationToken cancellationToken)
    {
        var response = await _service.ListLectureContentsAsync(request.LectureId); 
        return _mapper.Map<LectureContentListDto>(response);
    }
}
