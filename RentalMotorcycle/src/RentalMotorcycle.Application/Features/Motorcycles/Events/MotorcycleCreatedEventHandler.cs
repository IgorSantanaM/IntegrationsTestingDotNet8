using MassTransit;
using MediatR;
using RentalMotorcycle.Domain.Motorcycles.Events;

namespace RentalMotorcycle.Application.Features.Motorcycles.Events
{
    public class MotorcycleCreatedEventHandler(IPublishEndpoint publishEndpoint) : INotificationHandler<MotorcycleCreated>
    {
        public async Task Handle(MotorcycleCreated notification, CancellationToken cancellationToken)
        {
            await publishEndpoint.Publish(notification, cancellationToken);
        }
    }
}
