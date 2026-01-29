using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using RentalMotorcycle.Domain.Core.Models; // Onde está Entity<T>
using RentalMotorcycle.Domain.Notifications;

namespace RentalMotorcycle.Infra.Data.Mappings.Mongo
{
    public static class MotorcycleNotificationMap
    {
        public static void Configure()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Entity<Guid>)))
            {
                BsonClassMap.RegisterClassMap<Entity<Guid>>(map =>
                {
                    map.AutoMap();
                    map.SetIsRootClass(true);

                    map.MapIdMember(x => x.Id)
                       .SetIdGenerator(GuidGenerator.Instance)
                       .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(MotorcycleNotification)))
            {
                BsonClassMap.RegisterClassMap<MotorcycleNotification>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);

                    map.MapProperty(m => m.MotorcycleId)
                        .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));

                    map.MapProperty(x => x.CreatedAt)
                        .SetElementName("created_at")
                        .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));
                });
            }
        }
    }
}