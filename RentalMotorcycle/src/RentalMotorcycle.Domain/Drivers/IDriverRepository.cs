using RentalMotorcycle.Domain.Core.Data;

namespace RentalMotorcycle.Domain.Drivers
{
    public interface IDriverRepository : IRepository<Driver, Guid>
    {
        Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken token = default);
        Task<bool> ExistsByCnhNumberAsync(string cnhNumber, CancellationToken token = default);
        Task<Driver?> GetByCnpjAsync(string cnpj, CancellationToken token = default);
    }
}
