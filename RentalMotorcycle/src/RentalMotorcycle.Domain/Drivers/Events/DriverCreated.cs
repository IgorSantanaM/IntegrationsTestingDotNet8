using RentalMotorcycle.Domain.Core.Models;

namespace RentalMotorcycle.Domain.Drivers.Events;

public record DriverCreated(Guid DriverId,
                            string Name,
                            string CNPJ,
                            DateTime BirthDate,
                            string CNHNumber,
                            string CNHType,
                            string? CNHImageUrl) : Event<Guid>(DriverId);
