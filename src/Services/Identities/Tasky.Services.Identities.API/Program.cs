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
services.AddIdentityDatabase(configuration);             //Registers the database context and related services
services.AddCqrs();                                      //Registes CQRS services
services.AddRepositories();                              //Registers repositories
services.AddJwt(configuration);                          //Registers JWT authentication services    
services.AddRateLimiting();                              //Registers rate limiting services
services.AddAuthorizationBuilder()                       //Registers authorization services and policies
        .SetFallbackPolicy(new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());
        

var app = builder.Build();

app.UseRateLimiting();                      //Enables rate limiting middleware
app.UseMiddleware<ExceptionHandling>();     //Enables custom exception handling middleware
app.UseLogging();                           //Enables request logging middleware
app.UseAuthentication();                    //Enables authentication middleware
app.UseAuthorization();
app.MapControllers();
app.Run();