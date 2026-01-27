using RentalMotorcycle.Domain.Core.Exceptions;
using RentalMotorcycle.Domain.Core.Models;
using RentalMotorcycle.Domain.Rentals.Events;

namespace RentalMotorcycle.Domain.Rentals
{
    public class Rental : Entity<Guid>, IAggregateRoot
    {
        public Guid DriverId { get; private set; }
        public Guid MotorcycleId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime ExpectedEndDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }
        public int PlanType { get; private set; }
        public decimal DailyRate { get; private set; }
        public decimal TotalCost { get; private set; }

        private Rental() { }

        public Rental(Guid driverId, Guid motorcycleId, int planType)
        {
            Id = Guid.NewGuid();
            DriverId = driverId;
            MotorcycleId = motorcycleId;
            PlanType = planType;

            StartDate = DateTime.UtcNow.AddDays(1).Date;

            SetPlanDetails(planType);
            ExpectedEndDate = StartDate.AddDays(PlanType).Date;
            AddDomainEvent(new RentalStarted(Id, DriverId, MotorcycleId, StartDate, ExpectedEndDate, DailyRate));
        }

        private void SetPlanDetails(int planType)
        {
            DailyRate = planType switch
            {
                7 => 30.00m,
                15 => 28.00m,
                30 => 22.00m,
                45 => 20.00m,
                50 => 18.00m,
                _ => throw new DomainException("Invalid plan type. Available plans: 7, 15, 30, 45, 50 days.")
            };
        }

        public void CalculateFinalPrice(DateTime returnDate)
        {
            DateTime returnDateOnly = returnDate.Date;

            if (returnDateOnly < StartDate)
                throw new DomainException("Return date cannot be earlier than start date.");

            ReturnDate = returnDateOnly;

            int rawDaysUsed = (ReturnDate.Value - StartDate).Days;
            int daysUsed = rawDaysUsed == 0 ? 1 : rawDaysUsed;

            decimal penaltyApplied = 0;

            if (ReturnDate < ExpectedEndDate)
            {
                int daysNotUsed = (ExpectedEndDate - ReturnDate.Value).Days;

                decimal penaltyPercentage = PlanType switch
                {
                    7 => 0.20m,  
                    15 => 0.40m, 
                    _ => 0m     
                };

                penaltyApplied = (daysNotUsed * DailyRate) * penaltyPercentage;
                TotalCost = (daysUsed * DailyRate) + penaltyApplied;
            }
            else if (ReturnDate > ExpectedEndDate)
            {
                int extraDays = (ReturnDate.Value - ExpectedEndDate).Days;

                penaltyApplied = extraDays * 50.00m;

                TotalCost = (PlanType * DailyRate) + penaltyApplied;
            }
            else
            {
                TotalCost = PlanType * DailyRate;
            }

            var rentalReturnedEvent = new RentalReturned(
                Id,
                DriverId,
                ReturnDate.Value,
                TotalCost,
                penaltyApplied,
                daysUsed
            );

            AddDomainEvent(rentalReturnedEvent);
        }
    }
}