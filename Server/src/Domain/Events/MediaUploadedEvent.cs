using backend.Domain.Identifiers;
using MediatR;

namespace backend.Domain.Events;

public record MediaUploadedEvent(CourseId CourseId, LectureContentId LectureContentId) : INotification;