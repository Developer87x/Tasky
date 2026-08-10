using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Tasky.BuildingBlocks.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var configuration = builder.Configuration;
var jwtSection = configuration.GetSection("JwtSettings");
var issuer = jwtSection["Issuer"] ?? throw new InvalidOperationException("JwtSettings:Issuer is missing.");
var secret = jwtSection["Secret"] ?? throw new InvalidOperationException("JwtSettings:Secret is missing.");
var audiences = jwtSection.GetSection("Audience").Get<string[]>() ?? throw new InvalidOperationException("JwtSettings:Audience is missing.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudiences = audiences,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("GatewayJwtBearer");

                logger.LogWarning(context.Exception, "JWT authentication failed for {Path}", context.HttpContext.Request.Path);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser())
    .AddPolicy("AdministratorsOnly", policy => policy.RequireRole(Permissions.Roles.Administrators))
    .AddPolicy("ProjectsRead", policy => policy.RequireClaim("Permission", "Projects.Read"))
    .AddPolicy("ProjectsWrite", policy => policy.RequireClaim("Permission", "Projects.Write"));

builder.Services.AddReverseProxy()
    .LoadFromConfig(configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.Use(async (context, next) =>
{
    var correlationId = context.Request.Headers.TryGetValue("X-Correlation-ID", out var headerValues) && !string.IsNullOrWhiteSpace(headerValues.FirstOrDefault())
        ? headerValues.First()!
        : Guid.NewGuid().ToString("N");

    context.Response.Headers["X-Correlation-ID"] = correlationId;

    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("GatewayRequest");
    using (logger.BeginScope(new Dictionary<string, object>
    {
        ["CorrelationId"] = correlationId,
        ["TraceIdentifier"] = context.TraceIdentifier
    }))
    {
        try
        {
            logger.LogInformation("Incoming {Method} {Path}", context.Request.Method, context.Request.Path);
            await next();
            logger.LogInformation("Completed {StatusCode} {Method} {Path}", context.Response.StatusCode, context.Request.Method, context.Request.Path);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled gateway failure for {Method} {Path}", context.Request.Method, context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(new
            {
                title = "Gateway failure",
                status = StatusCodes.Status500InternalServerError,
                detail = "The gateway failed before the request could be proxied.",
                instance = context.Request.Path
            });
        }
    }
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();
app.Run();
