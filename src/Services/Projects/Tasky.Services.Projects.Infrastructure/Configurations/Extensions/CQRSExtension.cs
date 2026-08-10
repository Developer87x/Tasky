using Microsoft.Extensions.DependencyInjection;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.BuildingBlocks.Core.Events;
using Tasky.Services.Projects.Application.Commands.CreateCategoryCommands;


namespace Tasky.Services.Projects.Infrastructure.Configurations.Extensions;

public static class CqrsExtension
{
    extension(IServiceCollection service)
    {
        public IServiceCollection AddCqrs()
        {
            service.AddScoped<ICommandDispatcher, CommandDispatcher>();
            var assembly = typeof(CreateCategoryCommandHandler).Assembly;
            var handlerTypes = assembly.GetTypes()
                .Where(t => t is { IsAbstract: false, IsInterface: false })
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
                    .Select(i => new { Interface = i, Implementation = t }));
            foreach (var handler in handlerTypes)
                service.AddScoped(handler.Interface, handler.Implementation);

            var domainHandlerTypes = assembly.GetTypes()
                .Where(t => t is { IsAbstract: false, IsInterface: false })
                .SelectMany(t => t.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDomainEventHandler<>))
                    .Select(i => new { Interface = i, Implementation = t }));
            foreach (var handler in domainHandlerTypes)
                service.AddScoped(handler.Interface, handler.Implementation);
            
            return service;
        }
        
    }
}   
