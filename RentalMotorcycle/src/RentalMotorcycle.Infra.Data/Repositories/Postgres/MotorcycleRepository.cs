using Microsoft.EntityFrameworkCore;
using RentalMotorcycle.Domain.Motorcycles;
using RentalMotorcycle.Infra.Data.Contexts;

namespace RentalMotorcycle.Infra.Data.Repositories.Postgres
{
    public class MotorcycleRepository(RentalMotorcycleContext context) :
        Repository<Motorcycle, Guid>(context),
        IMotorcycleRepository
    {
        public async Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken token = default)
        {
            return await context.Motorcycles
                .AsNoTracking()
                .AnyAsync(m => EF.Functions.ILike(m.LicensePlate, licensePlate), token);
        }

        public async Task<IEnumerable<Motorcycle>?> GetAllAsync(string? licensePlateFilter = null, CancellationToken token = default)
        {
            var query = context.Motorcycles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(licensePlateFilter))
                query = query.Where(m => EF.Functions.ILike(m.LicensePlate, $"%{licensePlateFilter}%"));

            return await query.ToListAsync();
        }

        public async Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate, CancellationToken token = default)
        {
            return await context.Motorcycles
                .AsNoTracking()
                .FirstOrDefaultAsync(m => EF.Functions.ILike(m.LicensePlate, licensePlate), token);
        }

        public async Task<bool> HasRentalsAsync(Guid motorcycleId, CancellationToken token = default)
        {
            return await context.Rentals
                .AsNoTracking()
                .AnyAsync(r => r.MotorcycleId == motorcycleId, token);
        }
    }
}
