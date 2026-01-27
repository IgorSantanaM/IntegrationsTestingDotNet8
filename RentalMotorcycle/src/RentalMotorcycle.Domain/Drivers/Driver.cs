#pragma warning disable

using RentalMotorcycle.Domain.Core.Exceptions;
using RentalMotorcycle.Domain.Core.Models;
using RentalMotorcycle.Domain.Drivers.Events;

namespace RentalMotorcycle.Domain.Drivers
{
    public class Driver : Entity<Guid>, IAggregateRoot
    {
        public string Name { get; private set; }
        public string CNPJ { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string CNHNumber { get; private set; }
        public string CNHType { get; private set; }
        public string? CNHImageUrl { get; private set; }

        /// <summary>
        /// Private constructor so EF can create the entity's instance.
        /// </summary>
        private Driver() { }

        public Driver(string name, string cnpj, DateTime birthDate, string cnhNumber, string cnhType, string? cnhImageUrl)
        {
            Id = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Name is required.");

            if (string.IsNullOrWhiteSpace(cnpj))
                throw new DomainException("CNPJ is required.");

            if (!IsValidCNHType(cnhType))
                throw new DomainException("Invalid CNH type. Valid types are A, B or A+B.");

            Name = name;
            CNPJ = cnpj;
            BirthDate = birthDate;
            CNHNumber = cnhNumber;
            CNHType = cnhType.ToUpper();
            CNHImageUrl = cnhImageUrl;

            var driverCreatedEvent = new DriverCreated(Id, Name, CNPJ, BirthDate, CNHNumber, CNHType, CNHImageUrl);
            AddDomainEvent(driverCreatedEvent);
        }

        public void UpdateCNHImage(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new DomainException("Image URL cannot be empty.");

            CNHImageUrl = imageUrl;
            var cnhImageUploadedEvent = new CNHImageUploaded(Id, CNHImageUrl, DateTime.UtcNow);
            AddDomainEvent(cnhImageUploadedEvent);
        }
        public bool CanRentMotorcycle() => CNHType.Contains("A");

        private bool IsValidCNHType(string type)
        {
            var validTypes = new[] { "A", "B", "A+B" };
            return validTypes.Contains(type.ToUpper());
        }
    }
}
