using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RentalMotorcycle.Infra.Data.Mappings.Mongo;

namespace RentalMotorcycle.Infra.CrossCutting.IoC
{
    public static class ServicesCollections
    {
        public static void AddMongoPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            MotorcycleNotificationMap.Configure();

            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = MongoClientSettings.FromConnectionString(configuration.GetConnectionString("MongoDBConnection"));
                return new MongoClient(settings);
            });

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("RentalMotorcycleNotifications");
            });
        }
    }
}
