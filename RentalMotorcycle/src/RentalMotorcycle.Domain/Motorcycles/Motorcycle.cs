#pragma warning disable

using RentalMotorcycle.Domain.Core.Exceptions;
using RentalMotorcycle.Domain.Core.Models;
using RentalMotorcycle.Domain.Motorcycles.Events;

namespace RentalMotorcycle.Domain.Motorcycles
{
    public class Motorcycle : Entity<Guid>, IAggregateRoot
    {
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string LicensePlate { get; private set; }

        /// <summary>
        /// Private constructor so EF can create the entity's instance.
        /// </summary>
        private Motorcycle() { }

        public Motorcycle(int year, string model, string licensePlate)
        {
            Id = Guid.NewGuid();

            if (string.IsNullOrWhiteSpace(model))
                throw new DomainException("The model cannot be empty.");

            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new DomainException("The license plate cannot be empty.");

            if (year <= 1885 || year > DateTime.UtcNow.Year)
                throw new DomainException("The year of the motorcycle must be in valid range.");

            Year = year;
            Model = model;
            LicensePlate = licensePlate;

            var motorcycleCreatedEvent = new MotorcycleCreated(Id, Year, Model, LicensePlate);
            AddDomainEvent(motorcycleCreatedEvent);
        }

        public void UpdateLicensePlate(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new DomainException("The license plate cannot be empty.");

            LicensePlate = licensePlate;
            var motorcycleLicensePlateUpdatedEvent = new MotorcycleLicensePlateUpdated(Id, LicensePlate);
            AddDomainEvent(motorcycleLicensePlateUpdatedEvent);
        }
    }
}
