using MediatR;
using Microsoft.AspNetCore.Mvc;
using RentalMotorcycle.Application.Features.Motorcycles.Commands.CreateMotorcycle;
using RentalMotorcycle.Application.Features.Motorcycles.Queries.GetAllMotorcycles2024;
using RentalMotorcycle.Application.Features.Motorcycles.Queries.GetAllMotorcyclesQuery;
using RentalMotorcycle.Presentation.WebAPI.Endpoints.Internal;

namespace RentalMotorcycle.Presentation.WebAPI.Endpoints
{
    public class MotorcycleEndpoints : IEndpoint
    {
        public static void DefineEndpoints(WebApplication app)
        {
            var group = app.MapGroup("/api/motorcycles");

            group.MapPost("/create", HandleCreateMotorcycleAsync);

            group.MapGet("/", HandleGetAllMotorcyclesAsync);

            group.MapGet("/2024", HandleGetAllMotorcyclesIn2024Async);
        }

        #region Handlers

        private static async Task<IResult> HandleCreateMotorcycleAsync(
            [FromBody] CreateMotorcycleCommand command,
            [FromServices] IMediator mediator)
        {
            var motorcycleId = await mediator.Send(command);

            if (motorcycleId == Guid.Empty)
                return Results.BadRequest("Failed to create motorcycle");

            return Results.Created($"/motorcycles/{motorcycleId}", new { id = motorcycleId });
        }

        private static async Task<IResult> HandleGetAllMotorcyclesAsync(
            [FromQuery] string? licensePlate,
            [FromServices] IMediator mediator)
        {
            var query = new GetAllMotorcyclesQuery(licensePlate);
            var motorcycles = await mediator.Send(query);

            return Results.Ok(motorcycles);
        }

        private static async Task<IResult> HandleGetAllMotorcyclesIn2024Async(
            [FromQuery] string? licensePlate,
            [FromServices] IMediator mediator)
        {
            var query = new GetAllMotorcycles2024Query(licensePlate);
            var motorcycles = await mediator.Send(query);

            return Results.Ok(motorcycles);
        }

        #endregion

    }
}
