using RentalMotorcycle.Domain.Core.Models;

namespace RentalMotorcycle.Domain.Motorcycles.Events;

public record MotorcycleCreated(Guid MotorcycleId, int Year, string Model, string LicensePlate) : Event<Guid>(MotorcycleId);
