using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RentalMotorcycle.Infra.Data.Contexts;
using RentalMotorcycle.Infra.Data.Mappings.Mongo;

namespace RentalMotorcycle.Infra.CrossCutting.IoC
{
    public static class ServicesCollections
    {
        public static IServiceCollection AddMongoPersistence(this IServiceCollection services, IConfiguration configuration)
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

            return services;
        }

        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                // x.AddConsumer<MotorcycleCreatedConsumer>();

                x.AddEntityFrameworkOutbox<RentalMotorcycleContext>(o =>
                {
                    o.UsePostgres();

                    o.UseBusOutbox();
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqConnection = configuration.GetConnectionString("RabbitMq");

                    cfg.Host(rabbitMqConnection);

                    cfg.UseRawJsonSerializer();

                    cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
