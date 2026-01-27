using RentalMotorcycle.Domain.Core.Models;

namespace RentalMotorcycle.Domain.Drivers.Events;

public record CNHImageUploaded(Guid DriverId, string CNHImageUrl, DateTime UploadedAt) : Event<Guid>(DriverId);
