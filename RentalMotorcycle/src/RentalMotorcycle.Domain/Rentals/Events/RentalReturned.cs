using RentalMotorcycle.Domain.Core.Models;

namespace RentalMotorcycle.Domain.Rentals.Events;

public record RentalReturned(Guid RentalId,
                            Guid DriverId,
                            DateTime ReturnDate,
                            decimal TotalCost,
                            decimal PenaltyApplied,
                            int DaysUsed) : Event<Guid>(RentalId);
