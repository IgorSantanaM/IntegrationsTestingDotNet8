namespace RentalMotorcycle.Domain.Notifications
{
    public interface IMotorcycleNotificationRepository
    {
        public Task AddAsync(MotorcycleNotification motorcycleNotification, CancellationToken token);
        Task<IEnumerable<MotorcycleNotification>?> GetAllAsync(string? licensePlateFilter = null, CancellationToken token = default);
        Task<MotorcycleNotification?> GetByIdAsync(Guid motorcycleId, CancellationToken token);
        Task<bool> ExistsByMotorcycleIdAsync(Guid motorcycleId, CancellationToken token);
    }
}
