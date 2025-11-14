using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Application.Courses.Queries.GetCourseById;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace backend.Application.Lectures.Queries.GetLectureContentStream;

[Authorize]
public record GetLectureContentStreamQuery(CourseId CourseId, LectureId LectureId, LectureContentId LectureContentId, ResourceId ResourceId) : IRequest<IFormFile>;

public class GetLectureContentStreamQueryHandler : IRequestHandler<GetLectureContentStreamQuery, IFormFile>
{
    private readonly ICourseService _service;
    private readonly IMapper _mapper;

    public GetLectureContentStreamQueryHandler(ICourseService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    public async Task<IFormFile> Handle(GetLectureContentStreamQuery request, CancellationToken cancellationToken)
    {
        return await _service.GetLectureContentStreamAsync(request.CourseId, request.LectureId, request.LectureContentId, request.ResourceId, cancellationToken);
    }
}
