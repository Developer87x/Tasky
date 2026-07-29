using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Tasky.Services.Projects.Infrastructure.Configurations.Extensions;

public static class LoggingExtension
{
    extension(WebApplicationBuilder builder)
    {
        public void AddLogging() => builder.Host.UseSerilog((hostingContext, configuration) =>
        {
            configuration.ReadFrom.Configuration(hostingContext.Configuration);
        });
    }

    extension(WebApplication app)
    {
        public IApplicationBuilder UseLogging()
        {
            app.UseSerilogRequestLogging();
            return app;
        }
    }
}
