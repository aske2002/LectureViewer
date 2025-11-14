using backend.Application.Lectures.Commands.CreateCourse;
using backend.Application.Lectures.Queries.GetLectureById;
using backend.Application.Lectures.Queries.GetLectureContentStream;
using backend.Domain.Enums;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend.Web.Endpoints;

public class Lectures : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetLectureDetails, "/{courseId}/{lectureId}")
            .MapGet(GetLectureContent, "/{courseId}/{lectureId}/contents/{lectureContentId}/{resourceId}")
            .MapPost(UploadLectureMedia, "/{courseId}/{lectureId}/media");
    }

    public async Task<Ok<LectureDto>> GetLectureDetails(ISender sender, CourseId courseId, LectureId lectureId)
    {
        var vm = await sender.Send(new GetLectureQuery(courseId, lectureId));
        return TypedResults.Ok(vm);
    }

    public async Task<FileStreamHttpResult> GetLectureContent(ISender sender, CourseId courseId, LectureId lectureId, LectureContentId lectureContentId, ResourceId resourceId, [FromQuery] string? download)
    {
        var vm = await sender.Send(new GetLectureContentStreamQuery(courseId, lectureId, lectureContentId, resourceId));

        if (!string.IsNullOrEmpty(download))
        {
            return TypedResults.File(vm.OpenReadStream(), vm.ContentType ?? "application/octet-stream", vm.FileName, enableRangeProcessing: true);
        }
        else
        {
            return TypedResults.File(vm.OpenReadStream(), vm.ContentType ?? "application/octet-stream", enableRangeProcessing: true);

        }
    }

    public async Task<Ok<LectureContentId>> UploadLectureMedia(ISender sender, CourseId courseId, LectureId lectureId, IFormFile file, string name, string? description, bool isMainContent = false)
    {
        var command = new UploadLectureMediaCommand()
        {
            CourseId = courseId,
            LectureId = lectureId,
            File = file,
            Name = name,
            Description = description,
            IsMainContent = isMainContent

        };
        var vm = await sender.Send(command);
        return TypedResults.Ok(vm.Id);
    }
}
