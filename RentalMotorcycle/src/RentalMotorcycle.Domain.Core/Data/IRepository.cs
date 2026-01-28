using RentalMotorcycle.Domain.Core.Models;

namespace RentalMotorcycle.Domain.Core.Data
{
    public interface IRepository<TEntity, in TId> where TEntity : IAggregateRoot where TId : notnull
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity?> GetByIdAsync(TId id);
    }
}
