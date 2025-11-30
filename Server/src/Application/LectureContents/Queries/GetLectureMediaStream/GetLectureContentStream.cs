using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Http;

namespace backend.Application.LectureContents.Queries.GetLectureContentStream;

[Authorize]
public record GetLectureContentStreamQuery(CourseId CourseId, LectureContentId LectureContentId, ResourceId ResourceId) : IRequest<IFormFile>;

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
        return await _service.GetLectureContentStreamAsync(request.CourseId, request.LectureContentId, request.ResourceId, cancellationToken);
    }
}
