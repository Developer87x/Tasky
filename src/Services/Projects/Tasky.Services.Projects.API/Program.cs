using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Tasky.BuildingBlocks.Constants;
using Tasky.Services.Projects.Infrastructure.Configurations.Extensions;
using Tasky.Services.Projects.Infrastructure.Configurations.Middleware;



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateBootstrapLogger();


var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

var services = builder.Services;
var configurations = builder.Configuration;
var jwtSection = configurations.GetSection("JwtSettings");
var jwtIssuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JwtSettings:Issuer is missing.");
var jwtSecret = jwtSection["Secret"] ?? throw new InvalidOperationException("JwtSettings:Secret is missing.");
var jwtAudience = jwtSection["Audience"] ?? throw new InvalidOperationException("JwtSettings:Audience is missing.");

services.AddControllers();
services.AddProjectDatabase(configurations);
services.AddCqrs();
services.AddAuthorizationBuilder()
    .AddPolicy("FullAccess", policy => policy
        .RequireAuthenticatedUser()
        .RequireClaim("Permission", Permissions.Permission.FullAccess))
    .SetFallbackPolicy(new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build());
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            NameClaimType = System.Security.Claims.ClaimTypes.Name,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("ProjectsJwtBearer");

                logger.LogWarning(context.Exception, "JWT authentication failed for {Path}", context.HttpContext.Request.Path);
                return Task.CompletedTask;
            }
        };
    });
services.AddRepositories();

var app = builder.Build();


app.UseMiddleware<ExceptionHandling>();                 // Catch all exceptions
app.UseLogging();                                        // Request logging for audit
app.UseRouting();                                        // Route matching
app.UseAuthentication();                                 // JWT validation and ClaimsPrincipal extraction
app.UseAuthorization();                                  // Policy-based authorization
app.MapControllers();                                    // Controller routing
app.Run();