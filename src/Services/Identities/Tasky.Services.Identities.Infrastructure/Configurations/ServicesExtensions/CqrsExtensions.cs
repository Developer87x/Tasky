using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Queries;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

public static class CqrsExtensions
{
    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddScoped<IRoleQueries>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return new RoleQueries(config.GetConnectionString("IdentityDbStr")!, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<RoleQueries>>());
        });


        services.AddScoped<IUserQueries>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            return new UserQueries(config.GetConnectionString("IdentityDbStr")!, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<UserQueries>>());
        });


        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        var assembly = typeof(ICommandDispatcher).Assembly;
        var handlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
                .Select(i => new { Interface = i, Implementation = t }));

        foreach (var handler in handlerTypes)
            services.AddScoped(handler.Interface, handler.Implementation);

        var domainHandlerTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                .Select(i => new { Interface = i, Implementation = t }));

        foreach (var handler in domainHandlerTypes)
            services.AddScoped(handler.Interface, handler.Implementation);

        return services;
    }
}
