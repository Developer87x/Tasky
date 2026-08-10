using Microsoft.Extensions.Logging;
using Tasky.BuildingBlocks.Core.Events;
using Tasky.Services.Identities.Domain.DomainEvents;

namespace Tasky.Services.Identities.Application.EventHandlers;

public class RoleCreatedEventHandler(ILogger<RoleCreatedEventHandler> logger)
    : IDomainEventHandler<RoleCreatedEvent>
{
    private readonly ILogger<RoleCreatedEventHandler> _logger = logger;
    public Task HandleAsync(RoleCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("RoleCreatedDomainEvent handled for RoleId: {RoleId}, RoleName: {RoleName}", domainEvent.RoleId, domainEvent.RoleName);
        return Task.CompletedTask;
    }
}