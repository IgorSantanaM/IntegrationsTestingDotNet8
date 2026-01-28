using Microsoft.EntityFrameworkCore;
using RentalMotorcycle.Domain.Core.Data;
using RentalMotorcycle.Domain.Core.Models;
using RentalMotorcycle.Infra.Data.Contexts;

namespace RentalMotorcycle.Infra.Data.Repositories
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class, IAggregateRoot
        where TId : notnull
    {
        protected RentalMotorcycleContext _context;
        protected DbSet<TEntity> _dbSet;

        public Repository(RentalMotorcycleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
        }
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);

            return Task.CompletedTask;
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await _dbSet.FindAsync(id).AsTask();
        }

        public Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);

            return Task.CompletedTask;
        }
    }
}
