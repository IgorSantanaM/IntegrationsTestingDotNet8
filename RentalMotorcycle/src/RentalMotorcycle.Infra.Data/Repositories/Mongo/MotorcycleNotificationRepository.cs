using MongoDB.Driver;
using RentalMotorcycle.Domain.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace RentalMotorcycle.Infra.Data.Repositories.Mongo
{
    public class MotorcycleNotificationRepository : IMotorcycleNotificationRepository
    {
        private readonly IMongoCollection<MotorcycleNotification> _collection;

        public MotorcycleNotificationRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<MotorcycleNotification>("MotorcycleNotifications");
        }

        public async Task AddAsync(MotorcycleNotification motorcycleNotification, CancellationToken token)
        {
            await _collection.InsertOneAsync(motorcycleNotification, cancellationToken: token);
        }

        public async Task<IEnumerable<MotorcycleNotification>?> GetAllAsync(string? licensePlateFilter = null, CancellationToken token = default)
        {
            var filter = Builders<MotorcycleNotification>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(licensePlateFilter))
            {
                filter = Builders<MotorcycleNotification>.Filter.Regex(
                    x => x.LicensePlate,
                    new MongoDB.Bson.BsonRegularExpression(licensePlateFilter, "i")
                );
            }

            return await _collection
                .Find(filter)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync(token);
        }

        public async Task<MotorcycleNotification?> GetByIdAsync(Guid motorcycleId, CancellationToken token)
        {
            return await _collection
                .Find(m => m.MotorcycleId == motorcycleId)
                .FirstOrDefaultAsync(token);
        }

        public async Task<bool> ExistsByMotorcycleIdAsync(Guid motorcycleId, CancellationToken token)
        {
            return await _collection
                .Find(n => n.MotorcycleId == motorcycleId)
                .AnyAsync(token);
        }
    }
}