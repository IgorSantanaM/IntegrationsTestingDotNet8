using RentalMotorcycle.Domain.Core.Data;

namespace RentalMotorcycle.Domain.Motorcycles
{
    public interface IMotorcycleRepository : IRepository<Motorcycle, Guid>
    {
        Task<bool> ExistsByLicensePlateAsync(string licensePlate, CancellationToken token = default);
        Task<IEnumerable<Motorcycle>> GetAllAsync(string? licensePlateFilter = null, CancellationToken token = default);
        Task<Motorcycle?> GetByLicensePlateAsync(string licensePlate, CancellationToken token = default);
        Task<bool> HasRentalsAsync(Guid motorcycleId, CancellationToken token = default);
    }
}