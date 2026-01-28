using Microsoft.EntityFrameworkCore;
using RentalMotorcycle.Domain.Rentals;
using RentalMotorcycle.Infra.Data.Contexts;

namespace RentalMotorcycle.Infra.Data.Repositories
{
    public class RentalRepository(RentalMotorcycleContext context)
        : Repository<Rental, Guid>(context),
          IRentalRepository
    {
        public async Task<bool> DriverHasOpenRentalAsync(Guid driverId, CancellationToken token = default)
        {
            return await context.Rentals
                .AsNoTracking()
                .AnyAsync(r => r.DriverId == driverId && r.ReturnDate == null);
        }

        public async Task<Rental?> GetOpenRentalByDriverIdAsync(Guid driverId, CancellationToken token = default)
        {
            return await context.Rentals
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.DriverId == driverId && r.ReturnDate == null);
        }

        public async Task<bool> IsMotorcycleRentedAsync(Guid motorcycleId, CancellationToken token = default)
        {
            return await context.Rentals
                 .AsNoTracking()
                 .AnyAsync(r => r.MotorcycleId == motorcycleId && r.ReturnDate == null);
        }
    }
}
