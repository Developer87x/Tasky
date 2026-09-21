using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Tasky.Services.Identities.Infrastructure.Configurations.Signing;

namespace Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

public static class JwtConfigurationExtension
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        var jwtSigningConfig = configuration.GetSection("JwtSettings");
        var signingType = jwtSigningConfig["SigningType"] ?? "Symmetric";

        // Register the appropriate signing key provider based on configuration
        RegisterSigningKeyProvider(services, configuration, signingType);

        // Configure JWT Bearer authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = !IsLocalDevelopment(configuration);
                options.SaveToken = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudiences = configuration.GetSection("JwtSettings:Audience").Get<string[]>(),
                    IssuerSigningKeyResolver = ResolveSigningKeys(services),
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = System.Security.Claims.ClaimTypes.Name,
                    RoleClaimType = System.Security.Claims.ClaimTypes.Role
                };

                // Enhanced JWT event logging for security
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

    /// <summary>
    /// Register the appropriate signing key provider based on configuration.
    /// </summary>
    private static void RegisterSigningKeyProvider(
        IServiceCollection services,
        IConfiguration configuration,
        string signingType)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var signingConfig = configuration.GetSection("JwtSettings");

        switch (signingType.ToLowerInvariant())
        {
            case "rsa":
                var certPath = signingConfig["CertificatePath"]
                    ?? throw new InvalidOperationException(
                        "JwtSigningConfig:CertificatePath is required for RSA signing");
                var certPassword = signingConfig["CertificatePassword"];
                services.AddSingleton<ISigningKeyProvider>(
                    new RsaSigningKeyProvider(certPath, certPassword));
                break;

            case "externaloidc":
                var authority = signingConfig["Authority"]
                    ?? throw new InvalidOperationException(
                        "JwtSigningConfig:Authority is required for External OIDC signing");
                services.AddSingleton<ISigningKeyProvider>(
                    new ExternalOidcSigningKeyProvider(authority));
                break;

            case "symmetric":
            default:
                var secret = jwtSettings["Secret"]
                    ?? throw new InvalidOperationException(
                        "JwtSettings:Secret is required for symmetric signing");
                services.AddSingleton<ISigningKeyProvider>(
                    new SymmetricSigningKeyProvider(secret));
                break;
        }
    }

    /// <summary>
    /// Create a key resolver that uses ISigningKeyProvider to resolve signing keys.
    /// Supports key rotation and multiple validation keys.
    /// </summary>
    private static IssuerSigningKeyResolver ResolveSigningKeys(IServiceCollection services)
    {
        return (token, securityToken, kid, validationParameters) =>
        {
            // Build a temporary service provider to access the signing key provider
            // In a real app, this would be injected directly
            var provider = services.BuildServiceProvider();
            var signingKeyProvider = provider.GetRequiredService<ISigningKeyProvider>();

            // Return all valid keys (supports key rotation)
            var validationKeys = signingKeyProvider.GetValidationKeysAsync()
                .GetAwaiter()
                .GetResult();

            return validationKeys;
        };
    }

    /// <summary>
    /// Determine if running in local development mode.
    /// </summary>
    private static bool IsLocalDevelopment(IConfiguration configuration)
    {
        var aspnetcoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        return aspnetcoreEnv?.Equals("Development", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
