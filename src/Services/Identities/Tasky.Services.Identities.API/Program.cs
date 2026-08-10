using Serilog;
using Tasky.Services.Identities.Application.Services;
using Tasky.Services.Identities.Infrastructure.Configurations.Middlewares;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;
using Tasky.Services.Identities.Infrastructure.Services;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

var configuration = builder.Configuration;
var services = builder.Services;

// Add services to the container
services.AddControllers();
services.AddIdentityDatabase(configuration);              // Database and repositories
services.AddCqrs();                                       // CQRS pipeline
services.AddRepositories();                               // Repository implementations
services.AddJwtAuthentication(configuration);             // JWT authentication with pluggable signing

// Register improved token generation service
services.AddScoped<ITokenService, TokenGenerationService>();

// Centralized authorization policy configuration
// All policies defined in one place for consistency and audit
services.AddApplicationAuthorization();

services.AddRateLimiting();                               // Rate limiting to prevent abuse

var app = builder.Build();

// === MIDDLEWARE PIPELINE: Defense-in-depth approach ===
// Order matters: exceptions → logging → rate limiting → authentication → authorization → routing → business logic
app.UseMiddleware<ExceptionHandling>();                 // Catch all exceptions
app.UseLogging();                                        // Request logging for audit
app.UseRateLimiting();                                   // Rate limiting enforcement
app.UseRouting();                                        // Route matching
app.UseAuthentication();                                 // JWT validation and ClaimsPrincipal extraction
app.UseAuthorization();                                  // Policy-based authorization
app.MapControllers();                                    // Controller routing
app.Run();