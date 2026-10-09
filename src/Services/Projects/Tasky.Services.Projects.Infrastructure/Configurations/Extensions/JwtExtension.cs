using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Tasky.BuildingBlocks.Constants;

namespace Tasky.Services.Projects.Infrastructure.Configurations.Extensions;

public static class JwtExtension
{
    extension (IServiceCollection services)
    {
        public IServiceCollection AddJwtAuthentication(IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = IsLocalDevelopment(configuration),
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudiences = configuration.GetSection("JwtSettings:Audience").Get<string[]>(),
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = System.Security.Claims.ClaimTypes.Name,
                    RoleClaimType = System.Security.Claims.ClaimTypes.Role
                };
                options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILogger<JwtBearerEvents>>();
                            logger.LogWarning(context.Exception,
                                "JWT authentication failed for {Path}. Exception: {ExceptionMessage}",
                                context.Request.Path,
                                context.Exception?.Message);
                            return Task.CompletedTask;
                        },

                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            var logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILogger<JwtBearerEvents>>();
                            logger.LogWarning("Missing or invalid JWT token for {Path}",
                                context.Request.Path);

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/problem+json";
                            await context.Response.WriteAsJsonAsync(new
                            {
                                type = "https://httpstatuses.com/401",
                                title = "Unauthorized",
                                status = StatusCodes.Status401Unauthorized,
                                detail = "Authentication is required. Provide a valid Bearer token.",
                                instance = context.Request.Path.Value
                            });
                        },

                        OnForbidden = async context =>
                        {
                            var logger = context.HttpContext.RequestServices
                                .GetRequiredService<ILogger<JwtBearerEvents>>();
                            logger.LogWarning("Forbidden access attempt for {Path}. User: {User}",
                                context.Request.Path,
                                context.HttpContext.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                                ?? "unknown");

                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/problem+json";
                            await context.Response.WriteAsJsonAsync(new
                            {
                                type = "https://httpstatuses.com/403",
                                title = "Forbidden",
                                status = StatusCodes.Status403Forbidden,
                                detail = "You do not have permission to access this resource.",
                                instance = context.Request.Path.Value
                            });
                        }
                    };
            });
            return services;
        }
    }

    private static bool IsLocalDevelopment(IConfiguration configuration)
    {
        var aspnetcoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return aspnetcoreEnv?.Equals("Development", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}