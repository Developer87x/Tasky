using Microsoft.Extensions.DependencyInjection;
using Tasky.Services.Projects.Application.Commands;
using Tasky.Services.Projects.Domain.SharedKernel;

namespace Tasky.Services.Projects.Infrastructure.Configurations.Extensions;

public static class CQRSExtension
{
    extension(IServiceCollection service)
    {
        public IServiceCollection AddCQRS()
        {
            service.AddScoped<ICommandDispatcher, CommandDispatcher>();
            var assembly = typeof(ICommandDispatcher).Assembly;
            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
                    .Select(i => new { Interface = i, Implementation = t }));
            foreach (var handler in handlerTypes)
                service.AddScoped(handler.Interface, handler.Implementation);

            var domainHandlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                    .Select(i => new { Interface = i, Implementation = t }));
            foreach (var handler in domainHandlerTypes)
                service.AddScoped(handler.Interface, handler.Implementation);
            
            return service;
        }
        
    }
}   