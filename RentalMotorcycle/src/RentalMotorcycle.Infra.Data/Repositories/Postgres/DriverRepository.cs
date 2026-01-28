using Microsoft.EntityFrameworkCore;
using RentalMotorcycle.Domain.Drivers;
using RentalMotorcycle.Infra.Data.Contexts;

namespace RentalMotorcycle.Infra.Data.Repositories.Postgres
{
    public class DriverRepository(RentalMotorcycleContext context) : Repository<Driver, Guid>(context), IDriverRepository
    {
        public Task<bool> ExistsByCnhNumberAsync(string cnhNumber, CancellationToken token = default)
        {
            return context.Drivers
                .AsNoTracking()
                .AnyAsync(d => d.CNHNumber == cnhNumber, token);
        }

        public Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken token = default)
        {
            return context.Drivers
                .AsNoTracking()
                .AnyAsync(d => d.CNPJ == cnpj, token);
        }

        public Task<Driver?> GetByCnpjAsync(string cnpj, CancellationToken token = default)
        {
            return context.Drivers
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.CNPJ == cnpj, token);
        }
    }
}
