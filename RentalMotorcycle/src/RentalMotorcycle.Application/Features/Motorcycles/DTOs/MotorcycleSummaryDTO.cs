using System;
using System.Collections.Generic;
using System.Text;

namespace RentalMotorcycle.Application.Features.Motorcycles.DTOs
{
    public record MotorcycleSummaryDTO(Guid Id, int Year, string Model, string LicensePlate);
}
