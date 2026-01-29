using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RentalMotorcycle.Application.Features.Motorcycles.Commands.CreateMotorcycle;
using RentalMotorcycle.Application.Features.Motorcycles.Validators;
using RentalMotorcycle.Domain.Core.Data;
using RentalMotorcycle.Domain.Drivers;
using RentalMotorcycle.Domain.Motorcycles;
using RentalMotorcycle.Domain.Notifications;
using RentalMotorcycle.Domain.Rentals;
using RentalMotorcycle.Infra.Data.Contexts;
using RentalMotorcycle.Infra.Data.Interceptors;
using RentalMotorcycle.Infra.Data.Mappings.Mongo;
using RentalMotorcycle.Infra.Data.Repositories.Mongo;
using RentalMotorcycle.Infra.Data.Repositories.Postgres;
using RentalMotorcycle.Infra.Data.UoW;
using RentalMotorcycle.Infra.Messaging.Consumers;
using System.Data;

namespace RentalMotorcycle.Infra.CrossCutting.IoC
{
    public static class ServicesCollections
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateMotorcycleCommandHandler).Assembly));

            services.AddValidatorsFromAssembly(typeof(CreateMotorcycleCommandValidator).Assembly);

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<PublishDomainEventsInterceptor>();

            services.AddDbContext<RentalMotorcycleContext>((sp, options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("rentalmotorcycledb"));
                options.AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>());
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IDriverRepository, DriverRepository>();
            services.AddScoped<IMotorcycleNotificationRepository, MotorcycleNotificationRepository>();

            return services;
        }

        public static IServiceCollection AddMongoPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            MotorcycleNotificationMap.Configure();

            services.AddSingleton<IMongoClient>(sp =>
            {
                var connectionString = configuration["MongoDBSettings:ConnectionString"];

                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException("MongoDBSettings:ConnectionString is missing");

                var settings = MongoClientSettings.FromConnectionString(connectionString);
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
                 x.AddConsumer<MotorcycleCreatedConsumer>();

                x.AddEntityFrameworkOutbox<RentalMotorcycleContext>(o =>
                {
                    o.UsePostgres();

                    o.QueryDelay = TimeSpan.FromSeconds(10);

                    o.QueryMessageLimit = 50;

                    o.DuplicateDetectionWindow = TimeSpan.FromMinutes(30);

                    o.UseBusOutbox(bo =>
                    {
                        bo.MessageDeliveryLimit = 50;
                    });
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqConnection = configuration.GetConnectionString("RabbitMQ");

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
