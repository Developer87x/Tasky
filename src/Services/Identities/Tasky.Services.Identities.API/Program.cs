using Serilog;
using Tasky.Services.Identities.Infrastructure.Configurations.Middlewares;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .CreateBootstrapLogger();
var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

var configuration = builder.Configuration;
var services = builder.Services;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
services.AddControllers();
services.AddIdentityDatabase(configuration);
services.AddCqrs();
services.AddRepositories();
services.AddJwt(configuration);
services.AddRateLimiting();
services.AddAuthorizationBuilder()
        .SetFallbackPolicy(new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());



var app = builder.Build();

app.UseRateLimiting();
app.UseMiddleware<ExceptionHandling>();
app.UseLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();