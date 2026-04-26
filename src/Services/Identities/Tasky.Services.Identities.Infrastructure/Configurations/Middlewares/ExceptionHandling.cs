using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Infrastructure.Configurations.Middlewares
{
    public class ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unhandled exception on {Methods} {Path}: {Message}",
                    context.Request.Method,
                    context.Request.Path,
                    ex.Message);

                if (context.Response.HasStarted)
                throw;

                await HandleExceptionAsync(context, ex);
            }
        }

        public static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                Domain.Exceptions.NotFoundException e => (StatusCodes.Status404NotFound, e.Message),
                Domain.Exceptions.BadRequestException e => (StatusCodes.Status400BadRequest, e.Message),
                Domain.Exceptions.UnauthorizedException e => (StatusCodes.Status401Unauthorized, e.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            var proplemDetails = new ProblemDetails
            {
                Type = $"https://httpstatuses.com/{statusCode}",
                Title = message,
                Status = statusCode,
                Detail = statusCode == StatusCodes.Status500InternalServerError ? "An unexpected error occurred. Please try again later." : message,
                Instance = context.Request.Path
            };

            context.Response.Clear();  // clear any partial response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = JsonSerializer.Serialize(proplemDetails,new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            await context.Response.WriteAsync(response);
        }
    }
}