using RentalMotorcycle.Domain.Core.Data;

namespace RentalMotorcycle.Domain.Rentals
{
    public interface IRentalRepository : IRepository<Rental, Guid>
    {
        Task<bool> IsMotorcycleRentedAsync(Guid motorcycleId, CancellationToken token = default);
        Task<bool> DriverHasOpenRentalAsync(Guid driverId, CancellationToken token = default);
        Task<Rental?> GetOpenRentalByDriverIdAsync(Guid driverId, CancellationToken token = default);
    }
}
