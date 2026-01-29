using MediatR;
using RentalMotorcycle.Application.Features.Motorcycles.DTOs;

namespace RentalMotorcycle.Application.Features.Motorcycles.Queries.GetAllMotorcyclesQuery;

public record GetAllMotorcyclesQuery(string? licensePlate) : IRequest<IEnumerable<MotorcycleSummaryDTO>>;
