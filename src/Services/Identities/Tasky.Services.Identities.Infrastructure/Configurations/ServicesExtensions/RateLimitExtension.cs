using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;

namespace Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

public static class RateLimitExtension
{
    public const string RateLimitPolicyForAuthenticatedUsers = "AuthenticatedUsersPolicy";
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRateLimiting()
        {
            services.AddRateLimiter(rateLimit =>
            {
                rateLimit.AddFixedWindowLimiter(RateLimitPolicyForAuthenticatedUsers, opt =>
                {
                    opt.PermitLimit = 5;
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0;
                });
                rateLimit.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                rateLimit.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ =>
                        {
                            var options = new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 25,
                                Window = TimeSpan.FromMinutes(1)
                            };
                            return options;
                        }));
            });
            return services;
        }
    }

    extension(WebApplication app)
    {
        public WebApplication UseRateLimiting()
        {
            app.UseRateLimiter();
            return app;
        }
    }
}