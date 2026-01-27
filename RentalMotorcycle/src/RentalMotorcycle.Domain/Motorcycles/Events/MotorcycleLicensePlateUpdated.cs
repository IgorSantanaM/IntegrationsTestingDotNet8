using RentalMotorcycle.Domain.Core.Models;

namespace RentalMotorcycle.Domain.Motorcycles.Events;

public record MotorcycleLicensePlateUpdated(Guid MotorcycleId, string LicensePlate) : Event<Guid>(MotorcycleId);
