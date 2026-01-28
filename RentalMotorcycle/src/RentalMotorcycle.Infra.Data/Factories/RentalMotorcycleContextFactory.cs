using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using RentalMotorcycle.Infra.Data.Contexts;

namespace RentalMotorcycle.Infra.Data.Factories
{
    public class RentalMotorcycleContextFactory : IDesignTimeDbContextFactory<RentalMotorcycleContext>
    {
        private const string CONNECTION_STRING_NAME = "rentalmotorcycledb";

        public RentalMotorcycleContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<RentalMotorcycleContext>();
            var connectionString = configuration.GetConnectionString(CONNECTION_STRING_NAME);

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException($"Could not find the connection string {CONNECTION_STRING_NAME}");

            optionsBuilder.UseNpgsql(connectionString);

            return new RentalMotorcycleContext(optionsBuilder.Options);
        }
    }
}
