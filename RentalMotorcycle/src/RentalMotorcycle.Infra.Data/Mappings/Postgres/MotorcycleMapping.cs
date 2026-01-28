using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalMotorcycle.Domain.Motorcycles;
using RentalMotorcycle.Infra.Data.Extensions;

namespace RentalMotorcycle.Infra.Data.Mappings.Postgres
{
    public class MotorcycleMapping : EntityTypeConfiguration<Motorcycle>
    {
        public override void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.ToTable("Motorcycles");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.LicensePlate)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasIndex(m => m.LicensePlate)
                .IsUnique();

            builder.Property(m => m.Model)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.Year)
                .IsRequired();
        }
    }
}
