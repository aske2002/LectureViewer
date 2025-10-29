using backend.Application.Common.Models;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Application.Resources.Queries.ListResources;
using backend.Application.Users.Queries.GetUserInfo;
using backend.Domain.Identifiers;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Web.Endpoints;

public class Resources : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(ListResources)
            .MapGet(GetResourceInfo, "/{resourceId}")
            .MapGet(GetResourceContent, "/{resourceId}/{fileName}");
    }

    public async Task<Ok<FilteredList<ResourceResponse>>> ListResources(ISender sender, [AsParameters] ListResourcesWithPaginationQuery query)
    {
        var result = await sender.Send(query);
        return TypedResults.Ok(result);
    }

    public async Task<Ok<ResourceResponse>> GetResourceInfo(ISender sender, ResourceId resourceId)
    {
        var vm = await sender.Send(new GetResourceByIdQuery(resourceId));
        return TypedResults.Ok(vm);
    }

    public async Task<FileContentHttpResult> GetResourceContent(ISender sender, ResourceId resourceId, string fileName)
    {
        byte[] file = await sender.Send(new GetResourceContentByIdQuery(resourceId));
        var contentType = MimeTypesMap.GetMimeType(fileName);
        return TypedResults.File(file, contentType, fileName);
    }
}
