using MediatR;
using RentalMotorcycle.Application.Features.Motorcycles.DTOs;

namespace RentalMotorcycle.Application.Features.Motorcycles.Queries.GetAllMotorcycles2024;

public record GetAllMotorcycles2024Query(string? LicensePlate) : IRequest<IEnumerable<MotorcycleSummaryDTO>>;
