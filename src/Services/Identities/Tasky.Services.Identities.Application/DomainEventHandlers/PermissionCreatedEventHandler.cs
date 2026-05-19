using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Application.bin.DomainEventHandlers;

public class PermissionCreatedEventHandler(ILogger<PermissionCreatedEventHandler> logger) : IDomainEventHandler<PermissionCreatedDomainEvent>
{
    public Task Handle(PermissionCreatedDomainEvent domainEvent)
    {
        logger.LogInformation("Permission with id {PermissionId} was created at {DateOccurred}", domainEvent.Id, domainEvent.DateOccurred);
        return Task.CompletedTask;
    }
}