using backend.Application.Common.Models;
using backend.Application.Countries.Queries.GetCountries;
using backend.Application.Destinations.Commands.CreateDestination;
using backend.Application.Destinations.Queries.GetDestinations;
using backend.Application.TodoLists.Commands.CreateTodoList;
using backend.Application.TodoLists.Commands.DeleteTodoList;
using backend.Application.TodoLists.Commands.UpdateTodoList;
using backend.Application.TodoLists.Queries.GetTodos;
using backend.Domain.Identifiers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend.Web.Endpoints;

public class Destinations : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetDestinations)
            .MapPost(CreateDestination)
            .MapGroup("/countries")
                .MapGet(GetCountries);
    }

    public async Task<Ok<List<CountryDto>>> GetCountries(ISender sender)
    {
        var vm = await sender.Send(new GetCountriesQuery());
        return TypedResults.Ok(vm);
    }

    public async Task<Ok<FilteredList<DestinationDto>>> GetDestinations(ISender sender, [AsParameters] GetDestinationsQuery query)
    {
        var vm = await sender.Send(query);
        return TypedResults.Ok(vm);
    }

    public async Task<Ok<DestinationId>> CreateDestination(ISender sender, CreateDestinationCommand command)
    {
        var vm = await sender.Send(command);
        return TypedResults.Ok(vm);
    }
}
