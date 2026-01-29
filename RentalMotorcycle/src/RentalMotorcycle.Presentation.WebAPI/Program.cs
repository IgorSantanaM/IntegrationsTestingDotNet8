using Microsoft.EntityFrameworkCore;
using RentalMotorcycle.Infra.CrossCutting.IoC;
using RentalMotorcycle.Infra.Data.Contexts;
using RentalMotorcycle.Presentation.WebAPI.Endpoints.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddApplicationServices();

builder.Services.AddMongoPersistence(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMassTransitConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Rental Motorcycle API V1");
        opt.DocumentTitle = "Rental Motorcycle Documentation";
        opt.DefaultModelExpandDepth(-1);
    });
}

app.UseRouting();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RentalMotorcycleContext>();

    await context.Database.MigrateAsync();
}

app.UseCors();

app.UseEndpoints<Program>();

app.MapGet("/", () => "Api is running.");

app.Run();