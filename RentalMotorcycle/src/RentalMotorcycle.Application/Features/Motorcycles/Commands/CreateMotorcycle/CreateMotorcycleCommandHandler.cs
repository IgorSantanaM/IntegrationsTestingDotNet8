using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalMotorcycle.Application.Exceptions;
using RentalMotorcycle.Domain.Core.Data;
using RentalMotorcycle.Domain.Motorcycles;

namespace RentalMotorcycle.Application.Features.Motorcycles.Commands.CreateMotorcycle
{
    public class CreateMotorcycleCommandHandler(IMotorcycleRepository repository, IUnitOfWork unitOfWork, IValidator<CreateMotorcycleCommand> validator) : IRequestHandler<CreateMotorcycleCommand, Guid>
    {
        public async Task<Guid> Handle(CreateMotorcycleCommand request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var licensePlateExists = await repository.ExistsByLicensePlateAsync(request.LicensePlate);

            if (licensePlateExists)
                throw new ConflictException("The license plate should be unique.");

            var motorcycle = new Motorcycle(request.Year, request.Model, request.LicensePlate);

            try
            {
                await repository.AddAsync(motorcycle);
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("Race Condition: Failed to create, license plate duplicated");
            }

            return motorcycle.Id;
        }
    }
}
