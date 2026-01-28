using MediatR;
using System.Windows.Input;

namespace RentalMotorcycle.Application.Features.Motorcycles.Commands.CreateMotorcycle;

public record CreateMotorcycleCommand(int Year, string Model, string LicensePlate) : IRequest<Guid>;
