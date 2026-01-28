using RentalMotorcycle.Domain.Core.Exceptions;
using RentalMotorcycle.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentalMotorcycle.Domain.Notifications
{
    public class MotorcycleNotification : Entity<Guid>, IAggregateRoot
    {
        public Guid MotorcycleId { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string LicensePlate { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Private constructor for the MongoDB Driver / Mapping
        /// </summary>
        private MotorcycleNotification() { }

        public MotorcycleNotification(Guid motorcycleId, int year, string model, string licensePlate)
        {
            Id = Guid.NewGuid();

            if (motorcycleId == Guid.Empty)
                throw new DomainException("MotorcycleId is required for notification.");

            if (string.IsNullOrWhiteSpace(model))
                throw new DomainException("Model is required for notification.");

            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new DomainException("LicensePlate is required for notification.");

            MotorcycleId = motorcycleId;
            Year = year;
            Model = model;
            LicensePlate = licensePlate;
            CreatedAt = DateTime.UtcNow;

            Message = $"A new motorcycle from the year {year} was registered: {model} ({licensePlate})";
        }
    }
}
