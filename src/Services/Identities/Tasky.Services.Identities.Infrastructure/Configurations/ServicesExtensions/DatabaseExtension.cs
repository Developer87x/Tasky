using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasky.Services.Identities.Application.Services;
using Tasky.Services.Identities.Domain.Repositories;
using Tasky.Services.Identities.Infrastructure.Configurations.Middlewares.Handlers;
using Tasky.Services.Identities.Infrastructure.Configurations.Middlewares.Providers;
using Tasky.Services.Identities.Infrastructure.Persistence;
using Tasky.Services.Identities.Infrastructure.Persistence.Repositories;
using Tasky.Services.Identities.Infrastructure.Services;

namespace Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

public static class DatabaseExtension
{
    public static IServiceCollection AddIdentityDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDb>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("IdentityDbStr"),
            s =>
            {
                s.MigrationsAssembly("Tasky.Services.Identities.Infrastructure");
                s.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                s.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        });
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        return services;
    }


    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));


        var jwtSettingsSection = configuration.GetSection("JwtSettings");
        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;  
        }).AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettingsSection["Issuer"],
                ValidAudience = jwtSettingsSection["Audience"],
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettingsSection["Secret"]!)),
                ClockSkew = TimeSpan.Zero
            };
            opt.Events = new JwtBearerEvents
            {
                // 401 — missing or invalid token
                OnChallenge = async context =>
                {
                    context.HandleResponse(); // suppress default response

                    var problem = new ProblemDetails
                    {
                        Type = "https://httpstatuses.com/401",
                        Title = "Unauthorized",
                        Status = StatusCodes.Status401Unauthorized,
                        Detail = "Authentication is required. Provide a valid Bearer token.",
                        Instance = context.Request.Path
                    };

                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(problem);
                },

                // 403 — authenticated but lacks permission
                OnForbidden = async context =>
                {
                    var problem = new ProblemDetails
                    {
                        Type = "https://httpstatuses.com/403",
                        Title = "Forbidden",
                        Status = StatusCodes.Status403Forbidden,
                        Detail = "You do not have permission to access this resource.",
                        Instance = context.Request.Path
                    };

                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsJsonAsync(problem);
                },

                // Optional: log token validation failures
                OnAuthenticationFailed = context =>
                {
                    // e.g., expired token, invalid signature
                    // context.Exception gives you the specific error
                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorization();
        return services;
    }
}
