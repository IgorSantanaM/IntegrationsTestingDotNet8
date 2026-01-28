using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using RentalMotorcycle.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentalMotorcycle.Infra.Data.Mappings.Mongo
{
    public static class MotorcycleNotificationMap
    {
        public static void Configure()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(MotorcycleNotification)))
            {
                BsonClassMap.RegisterClassMap<MotorcycleNotification>(map =>
                {
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);

                    map.MapIdField(m => m.Id)
                        .SetIdGenerator(GuidGenerator.Instance)
                        .SetSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

                    map.MapProperty(m => m.MotorcycleId)
                        .SetSerializer(new GuidSerializer(MongoDB.Bson.GuidRepresentation.Standard));

                    map.MapProperty(x => x.CreatedAt)
                       .SetElementName("created_at")
                       .SetSerializer(new DateTimeSerializer(DateTimeKind.Utc));
                });
            }
        }
    }
}
