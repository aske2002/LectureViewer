using backend.Application.LectureContents.Queries.GetLectureContentStream;
using backend.Application.LectureContents.Queries.ListLectureContents;
using backend.Application.Lectures.Commands.UpdateLectureContent;
using backend.Application.Lectures.Commands.UploadLectureContent;
using backend.Application.Lectures.Queries.GetLectureById;
using backend.Application.Transcriptions.Commands.RequestKeywordExtraction;
using backend.Domain.Constants;
using backend.Domain.Entities;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Authorization;
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
            .MapPost(ExtractTranscriptKeywords, "/transcripts/{transcriptId}/extract-keywords")
            .MapGet(GetLectureContentStream, "/{courseId}/{lectureId}/contents/{lectureContentId}/{resourceId}")
            .MapGet(ListLectureContents, "/{courseId}/{lectureId}/contents")
            .MapPost(UploadLectureMedia, "/{courseId}/{lectureId}/media")
            .MapPatch(UpdateLectureContent, "/{courseId}/{lectureId}/contents/{lectureContentId}");
    }

    public async Task<NoContent> ExtractTranscriptKeywords(ISender sender, TranscriptId transcriptId)
    {
        await sender.Send(new RequestKeywordExtractionCommand(transcriptId));
        return TypedResults.NoContent();
    }

    [Authorize(Policy = Policies.ViewCourse)]
    public async Task<Ok<LectureDto>> GetLectureDetails(ISender sender, CourseId courseId, LectureId lectureId)
    {
        var vm = await sender.Send(new GetLectureQuery(courseId, lectureId));
        return TypedResults.Ok(vm);
    }

    [Authorize(Policy = Policies.EditCourse)]
    public async Task<Ok<LectureContentDto>> UpdateLectureContent(ISender sender, CourseId courseId, [FromBody] UpdateLectureContentCommandContent command)
    {
        var vm = await sender.Send(new UpdateLectureContentCommand()
        {
            CourseId = courseId,
            Command = command
        });
        return TypedResults.Ok(vm);
    }

    [Authorize(Policy = Policies.ViewCourse)]
    public async Task<FileStreamHttpResult> GetLectureContentStream(ISender sender, CourseId courseId, LectureId lectureId, LectureContentId lectureContentId, ResourceId resourceId, [FromQuery] string? download)
    {
        var vm = await sender.Send(new GetLectureContentStreamQuery(courseId, lectureContentId, resourceId));

        if (!string.IsNullOrEmpty(download))
        {
            return TypedResults.File(vm.OpenReadStream(), vm.ContentType ?? "application/octet-stream", vm.FileName, enableRangeProcessing: true);
        }
        else
        {
            return TypedResults.File(vm.OpenReadStream(), vm.ContentType ?? "application/octet-stream", enableRangeProcessing: true);

        }
    }

    [Authorize(Policy = Policies.ViewCourse)]
    public async Task<Ok<LectureContentListDto>> ListLectureContents(ISender sender, CourseId courseId, LectureId lectureId)
    {
        var query = new ListLectureContentsQuery(courseId, lectureId);
        var vm = await sender.Send(query);
        return TypedResults.Ok(vm);
    }

    [Authorize(Policy = Policies.UploadCourseContent)]
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
