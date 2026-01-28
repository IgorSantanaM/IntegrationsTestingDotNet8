using MassTransit;
using Microsoft.EntityFrameworkCore;
using RentalMotorcycle.Domain.Drivers;
using RentalMotorcycle.Domain.Motorcycles;
using RentalMotorcycle.Domain.Rentals;

namespace RentalMotorcycle.Infra.Data.Contexts
{
    public class RentalMotorcycleContext : DbContext
    {
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Domain.Core.Models.Event<Guid>>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RentalMotorcycleContext).Assembly);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            base.OnModelCreating(modelBuilder);
        }
    }
}
