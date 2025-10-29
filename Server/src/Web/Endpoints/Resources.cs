using backend.Application.Countries.Queries.GetCountries;
using backend.Application.Resources.Queries.GetResourceById;
using backend.Application.TodoLists.Commands.CreateTodoList;
using backend.Application.TodoLists.Commands.DeleteTodoList;
using backend.Application.TodoLists.Commands.UpdateTodoList;
using backend.Application.TodoLists.Queries.GetTodos;
using backend.Application.Users.Queries.GetUserInfo;
using backend.Domain.Identifiers;
using HeyRed.Mime;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend.Web.Endpoints;

public class Resources : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetResourceInfo, "/{resourceId}")
            .MapGet(GetResourceContent, "/{resourceId}/{fileName}");
    }

    public async Task<Ok<Application.Resources.Queries.GetResourceById.ResourceResponse>> GetResourceInfo(ISender sender, ResourceId resourceId)
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
