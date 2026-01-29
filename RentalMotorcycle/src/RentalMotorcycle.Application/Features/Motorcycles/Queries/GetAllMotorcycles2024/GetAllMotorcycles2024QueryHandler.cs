using MediatR;
using RentalMotorcycle.Application.Features.Motorcycles.DTOs;
using RentalMotorcycle.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentalMotorcycle.Application.Features.Motorcycles.Queries.GetAllMotorcycles2024
{
    public class GetAllMotorcycles2024QueryHandler(IMotorcycleNotificationRepository repository) : IRequestHandler<GetAllMotorcycles2024Query, IEnumerable<MotorcycleSummaryDTO>>
    {
        public async Task<IEnumerable<MotorcycleSummaryDTO>> Handle(GetAllMotorcycles2024Query request, CancellationToken cancellationToken)
        {
            var motorcyclesCollection = await repository.GetAllAsync(request.LicensePlate, cancellationToken);

            if(motorcyclesCollection == null)
                return Enumerable.Empty<MotorcycleSummaryDTO>();

            return motorcyclesCollection.Select(m => new MotorcycleSummaryDTO(m.MotorcycleId, m.Year, m.Model, m.LicensePlate));
        }
    }
}
