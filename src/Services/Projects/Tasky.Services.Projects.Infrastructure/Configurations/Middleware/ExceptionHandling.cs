using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tasky.Services.Projects.Domain.Exceptions;  

namespace Tasky.Services.Projects.Infrastructure.Configurations.Middleware;

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
            logger.LogError(ex, "ExceptionHandling CAUGHT exception: {Type} - {Message}",
                ex.GetType().Name, ex.Message);

            if (context.Response.HasStarted)
            {
                logger.LogError("Response already started, cannot handle exception");
                throw;
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    public static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            NotFoundException e => (StatusCodes.Status404NotFound, e.Message),
            BadRequestException e => (StatusCodes.Status400BadRequest, e.Message),
            UnauthorizedException e => (StatusCodes.Status401Unauthorized, e.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        var problemDetails = new ProblemDetails  
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = message,
            Status = statusCode,
            Detail = statusCode == StatusCodes.Status500InternalServerError 
                ? "An unexpected error occurred. Please try again later." 
                : message,
            Instance = context.Request.Path
        };

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = JsonSerializer.Serialize(problemDetails, 
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(response);
    }
}