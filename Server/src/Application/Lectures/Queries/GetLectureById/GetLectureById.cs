using backend.Application.Common.Interfaces;
using backend.Application.Common.Security;
using backend.Application.LectureContents.Queries.ListLectureContents;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using backend.Domain.Interfaces;

namespace backend.Application.Lectures.Queries.GetLectureById;

[Authorize]
public record GetLectureQuery(CourseId CourseId, LectureId LectureId) : IRequest<LectureDto>;

public class GetLectureQueryHandler : IRequestHandler<GetLectureQuery, LectureDto>
{
    private readonly IRepository<LectureContent, LectureContentId> _lectureContentRepository;
    private readonly ICourseService _service;
    private readonly IMapper _mapper;

    public GetLectureQueryHandler(ICourseService service, IMapper mapper, IRepository<LectureContent, LectureContentId> lectureContentRepository)
    {
        _service = service;
        _mapper = mapper;
        _lectureContentRepository = lectureContentRepository;
    }

    public async Task<LectureDto> Handle(GetLectureQuery request, CancellationToken cancellationToken)
    {
        var lectureDetails = await _service.GetLectureDetailsAsync(request.CourseId, request.LectureId);
        var lectureVideoContent = await _lectureContentRepository.QueryAsync(
            filter: lc => lc.Where(lc => lc.LectureId == request.LectureId && lc.ContentType == Domain.Enums.LectureContentType.Video && lc.IsMainContent),
            include: lc => lc.Include(lc => lc.Resource).Include(lc => lc.Transcript!).ThenInclude(t => t.Items)
        );
        var lecturePresentationContent = await _lectureContentRepository.FindAsync(
            filter: lc => lc.Where(lc => lc.LectureId == request.LectureId && lc.ContentType == Domain.Enums.LectureContentType.Presentation && lc.IsMainContent),
            include: lc => lc.Include(lc => lc.Resource)
        );
        var lectureTranscription = lectureVideoContent.Entities.FirstOrDefault()?.Transcript;


        var response = _mapper.Map<LectureDto>(lectureDetails);
        response.PrimaryResource = new LecturePrimaryResourceDto
        {
            Presentation = lecturePresentationContent != null ? _mapper.Map<LectureContentDto>(lecturePresentationContent) : null,
            Media = lectureVideoContent.Entities.FirstOrDefault() != null ? _mapper.Map<LectureContentDto>(lectureVideoContent.Entities.FirstOrDefault()!) : null,
            Transcription = lectureTranscription != null ? _mapper.Map<TranscriptionDto>(lectureTranscription) : null
        };
        return response;
    }
}
