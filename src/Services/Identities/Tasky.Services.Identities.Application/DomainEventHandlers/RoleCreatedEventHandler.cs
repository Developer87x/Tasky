using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Application.bin.DomainEventHandlers;

public class RoleCreatedEventHandler(ILogger<RoleCreatedEventHandler> logger) : IDomainEventHandler<RoleCreatedDomainEvent>
{
    public Task Handle(RoleCreatedDomainEvent domainEvent)
    {
        logger.LogInformation("Role with id {RoleId} was created at {DateOccurred}", domainEvent.Id, domainEvent.DateOccurred);
        return Task.CompletedTask;
    }
}
