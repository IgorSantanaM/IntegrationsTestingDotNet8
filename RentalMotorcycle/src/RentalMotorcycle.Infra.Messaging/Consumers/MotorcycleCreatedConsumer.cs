
using MassTransit;
using RentalMotorcycle.Domain.Motorcycles.Events;
using RentalMotorcycle.Domain.Notifications;

namespace RentalMotorcycle.Infra.Messaging.Consumers
{
    public class MotorcycleCreatedConsumer(IMotorcycleNotificationRepository repository) : IConsumer<MotorcycleCreated>
    {
        public async Task Consume(ConsumeContext<MotorcycleCreated> context)
        {
            var message = context.Message;

            if (message.Year != 2024)
                return;

            var alreadyExists = await repository.ExistsByMotorcycleIdAsync(message.MotorcycleId, context.CancellationToken);
            if (alreadyExists)
            {
                return;
            }

            var motorcycleNotification = new MotorcycleNotification(
                message.MotorcycleId,
                message.Year,
                message.Model,
                message.LicensePlate);

            await repository.AddAsync(motorcycleNotification, context.CancellationToken);
        }
    }
}
