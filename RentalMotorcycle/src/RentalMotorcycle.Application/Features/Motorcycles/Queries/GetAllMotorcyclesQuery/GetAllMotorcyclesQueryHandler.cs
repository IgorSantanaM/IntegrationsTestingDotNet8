using MediatR;
using RentalMotorcycle.Application.Features.Motorcycles.DTOs;
using RentalMotorcycle.Domain.Motorcycles;

namespace RentalMotorcycle.Application.Features.Motorcycles.Queries.GetAllMotorcyclesQuery
{
    public class GetAllMotorcyclesQueryHandler(IMotorcycleRepository repository) : IRequestHandler<GetAllMotorcyclesQuery, IEnumerable<MotorcycleSummaryDTO>>
    {
        public async Task<IEnumerable<MotorcycleSummaryDTO>> Handle(GetAllMotorcyclesQuery request, CancellationToken cancellationToken)
        {
            var motorcycles = await repository.GetAllAsync(request.licensePlate, cancellationToken);

            if (motorcycles == null)
                return Enumerable.Empty<MotorcycleSummaryDTO>();

            return motorcycles.Select(m => new MotorcycleSummaryDTO(m.Id, m.Year, m.Model, m.LicensePlate));
        }
    }
}
