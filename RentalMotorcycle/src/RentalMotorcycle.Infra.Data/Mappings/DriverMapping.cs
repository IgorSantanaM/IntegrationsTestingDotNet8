using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentalMotorcycle.Domain.Drivers;
using RentalMotorcycle.Infra.Data.Extensions;

namespace RentalMotorcycle.Infra.Data.Mappings
{
    public class DriverMapping : EntityTypeConfiguration<Driver>
    {
        public override void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("Drivers");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(d => d.CNPJ)
                .IsRequired()
                .HasMaxLength(14) 
                .IsFixedLength(); 

            builder.Property(d => d.CNHNumber)
                .IsRequired()
                .HasMaxLength(11) 
                .IsFixedLength();

            builder.Property(d => d.CNHType)
                .IsRequired()
                .HasMaxLength(5); 

            builder.Property(d => d.CNHImageUrl)
                .IsRequired(false) 
                .HasMaxLength(500);

            builder.Property(d => d.BirthDate)
                .IsRequired()
                .HasColumnType("date");

            builder.HasIndex(d => d.CNPJ)
                .IsUnique();

            builder.HasIndex(d => d.CNHNumber)
                .IsUnique();
        }
    }
}
