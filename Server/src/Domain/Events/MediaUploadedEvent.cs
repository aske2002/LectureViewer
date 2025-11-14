using backend.Domain.Identifiers;
using MediatR;

namespace backend.Domain.Events;

public record MediaUploadedEvent(CourseId CourseId, LectureId LectureId, LectureContentId LectureContentId) : INotification;