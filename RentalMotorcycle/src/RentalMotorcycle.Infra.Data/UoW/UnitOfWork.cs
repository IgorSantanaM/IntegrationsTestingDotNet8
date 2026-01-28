using RentalMotorcycle.Domain.Core.Data;
using RentalMotorcycle.Infra.Data.Contexts;

namespace RentalMotorcycle.Infra.Data.UoW
{
    public class UnitOfWork(RentalMotorcycleContext context) : IUnitOfWork, IDisposable
    {
        private bool _disposed;
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
         => await context.SaveChangesAsync(cancellationToken);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                context.Dispose();
            }
            _disposed = true;
        }
    }
}
