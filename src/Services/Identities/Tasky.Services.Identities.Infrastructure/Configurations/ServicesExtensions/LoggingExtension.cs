using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions
{
    public static class LoggingExtension
    {
    
        public static void AddLogging(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((hostingContext, configuration) =>
            {
                configuration.ReadFrom.Configuration(hostingContext.Configuration);
            });
        }

        public static IApplicationBuilder UseLogging(this WebApplication app)
        {
            app.UseSerilogRequestLogging();
            return app;
        }
    }
}
