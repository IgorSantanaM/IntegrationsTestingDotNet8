using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalMotorcycle.Domain.Drivers;
using RentalMotorcycle.Domain.Motorcycles;
using RentalMotorcycle.Domain.Rentals;
using RentalMotorcycle.Infra.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentalMotorcycle.Infra.Data.Mappings
{
    public class RentalMapping : EntityTypeConfiguration<Rental>
    {
        public override void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("Rentals");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.PlanType)
                .IsRequired();

            builder.Property(r => r.DailyRate)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(r => r.StartDate)
                .IsRequired();

            builder.Property(r => r.ExpectedEndDate)
                .IsRequired();

            builder.Property(r => r.ReturnDate)
                .IsRequired(false);

            builder.HasOne<Driver>()
                .WithMany()
                .HasForeignKey(r => r.DriverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Motorcycle>()
                .WithMany() // TODO: check the rules and see if its one to one or one to many
                .HasForeignKey(r => r.MotorcycleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
