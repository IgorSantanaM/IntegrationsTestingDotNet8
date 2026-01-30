using FluentValidation;
using MassTransit.NewIdProviders;
using Microsoft.EntityFrameworkCore;
using RentalMotorcycle.Application.Exceptions;
using RentalMotorcycle.Domain.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace RentalMotorcycle.Presentation.WebAPI.Middlewares
{
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title, errors) = exception switch
            {
                ArgumentException argumentException => (
                    HttpStatusCode.BadRequest,
                    argumentException.Message,
                    null
                ),
                ValidationException validationException => (
                    HttpStatusCode.BadRequest,
                    "Validation failed",
                    validationException.Errors.Select(e => new {field = e.PropertyName, error = e.ErrorMessage})
                ),
                DomainException domainException => (
                    HttpStatusCode.UnprocessableEntity,
                    domainException.Message,
                    null
                ),
                NotFoundException notFoundException => (
                    HttpStatusCode.NotFound,
                    notFoundException.Message,
                    null
                ),
                ConflictException conflictException => (
                    HttpStatusCode.Conflict,
                    "Conflic detected",
                    new[] { new {field = "State", error = conflictException.Message}}
                ),
                BadHttpRequestException badHttpRequestException => (
                    HttpStatusCode.BadRequest,
                    badHttpRequestException.Message,
                    null
                ),
                _ => (
                    HttpStatusCode.InternalServerError,
                    "An unexpected error occurred, try again later.",
                    null
                )
            };

            var problem = new
            {
                Status = (int)statusCode,
                Title = title,
                Errors = errors
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = problem.Status;

            var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            });
            
            await context.Response.WriteAsync(json);
        }
    }
}
