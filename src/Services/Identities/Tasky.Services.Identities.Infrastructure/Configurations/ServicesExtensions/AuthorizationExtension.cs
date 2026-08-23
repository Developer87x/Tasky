using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Tasky.Services.Identities.Infrastructure.Configurations.Middlewares.Handlers;
using Tasky.Services.Identities.Infrastructure.Configurations.Middlewares.Providers;

namespace Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;


public static class AuthorizationExtension
{

    public static IServiceCollection AddApplicationAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()

            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}
