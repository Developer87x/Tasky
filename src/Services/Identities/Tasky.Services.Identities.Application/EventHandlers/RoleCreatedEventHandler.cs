using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Application.EventHandlers;

public class RoleCreatedEventHandler(ILogger<RoleCreatedEventHandler> logger) : IDomainEventHandler<RoleCreatedDomainEvent>
{
    private readonly ILogger<RoleCreatedEventHandler> _logger = logger;
    public Task Handle(RoleCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("RoleCreatedDomainEvent handled for RoleId: {RoleId}, RoleName: {RoleName}", domainEvent.RoleId, domainEvent.RoleName);
        return Task.CompletedTask;
    }
}