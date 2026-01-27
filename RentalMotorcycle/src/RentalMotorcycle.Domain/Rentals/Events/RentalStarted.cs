using RentalMotorcycle.Domain.Core.Models;

namespace RentalMotorcycle.Domain.Rentals.Events;

public record RentalStarted(Guid RentalId,
                            Guid DriverId,
                            Guid MotorcycleId,
                            DateTime StartDate,
                            DateTime ExpectedEndDate,
                            decimal DailyRate) : Event<Guid>(RentalId);
