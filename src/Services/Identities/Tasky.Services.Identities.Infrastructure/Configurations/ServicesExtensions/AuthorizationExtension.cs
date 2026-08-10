using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Tasky.Services.Identities.Application.Security;
using Tasky.Services.Identities.Domain.Entities;
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
                .Build())

            .AddPolicy(Permissions.Users.Read,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Users.Read))

            .AddPolicy(Permissions.Users.Create,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Users.Create))

            .AddPolicy(Permissions.Users.Update,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Users.Update))

            .AddPolicy(Permissions.Users.Delete,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Users.Delete))

            .AddPolicy(Permissions.Users.AssignRole,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Users.AssignRole))

            .AddPolicy(Permissions.Users.RemoveRole,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Users.RemoveRole))

            .AddPolicy(Permissions.Users.Activate,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Users.Activate))

            .AddPolicy(Permissions.Users.Deactivate,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Users.Deactivate))

            .AddPolicy(Permissions.Users.ResetPassword,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Users.ResetPassword))


            .AddPolicy(Permissions.Roles.Read,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Roles.Read))

            .AddPolicy(Permissions.Roles.Create,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Roles.Create))

            .AddPolicy(Permissions.Roles.Update,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Roles.Update))

            .AddPolicy(Permissions.Roles.Delete,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Roles.Delete))

            .AddPolicy(Permissions.Roles.AssignPermissions,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Roles.AssignPermissions))


            .AddPolicy(Permissions.PermissionManagement.Read,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.PermissionManagement.Read))

            .AddPolicy(Permissions.PermissionManagement.Create,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.PermissionManagement.Create))

            .AddPolicy(Permissions.PermissionManagement.Update,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.PermissionManagement.Update))

            .AddPolicy(Permissions.PermissionManagement.Delete,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.PermissionManagement.Delete))


            .AddPolicy(Permissions.Projects.Read,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Projects.Read))

            .AddPolicy(Permissions.Projects.Create,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Projects.Create))

            .AddPolicy(Permissions.Projects.Update,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Projects.Update))

            .AddPolicy(Permissions.Projects.Delete,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Projects.Delete))

            .AddPolicy(Permissions.Projects.Archive,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Projects.Archive))

            .AddPolicy(Permissions.Tasks.Read,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Tasks.Read))
            .AddPolicy(Permissions.Tasks.Create,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Tasks.Create))

            .AddPolicy(Permissions.Tasks.Update,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Tasks.Update))

            .AddPolicy(Permissions.Tasks.Delete,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Tasks.Delete))

            .AddPolicy(Permissions.Tasks.Complete,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Tasks.Complete))


            .AddPolicy(Permissions.Administration.FullAccess,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Administration.FullAccess))

            .AddPolicy(Permissions.Administration.Audit,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Administration.Audit))

            .AddPolicy(Permissions.Administration.ManageConfiguration,
                policy => policy
                    .RequireAuthenticatedUser()
                    .RequireClaim("Permission", Permissions.Administration.ManageConfiguration));
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}
