using backend.Application.Common.Interfaces;
using backend.Application.LectureContents.Queries.ListLectureContents;
using backend.Domain.Entities;
using backend.Domain.Identifiers;

namespace backend.Application.Lectures.Commands.UpdateLectureContent;

public record UpdateLectureContentCommandContent
{
    public required LectureContentId LectureContentId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public bool? IsMainContent { get; init; }
}

public record UpdateLectureContentCommand : WithCourseId<UpdateLectureContentCommandContent, LectureContentDto>;

public class UpdateLectureContentCommandHandler : IRequestHandler<UpdateLectureContentCommand, LectureContentDto>
{
    private readonly ICourseService _courseService;
    private readonly IMapper _mapper;

    public UpdateLectureContentCommandHandler(ICourseService courseService, IMapper mapper)
    {
        _courseService = courseService;
        _mapper = mapper;
    }

    public async Task<LectureContentDto> Handle(UpdateLectureContentCommand request, CancellationToken cancellationToken)
    {
        var updatedLectureContent = await _courseService.UpdateLectureContentAsync(request.CourseId, request.Command.LectureContentId, request.Command.Name, request.Command.Description, request.Command.IsMainContent);
        return _mapper.Map<LectureContentDto>(updatedLectureContent);
    }
}
