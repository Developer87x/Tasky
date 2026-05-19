using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Application.bin.DomainEventHandlers;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger) : IDomainEventHandler<UserCreatedDomainEvent>
{
    public Task Handle(UserCreatedDomainEvent domainEvent)
    {
        logger.LogInformation("User with id {UserId} was created at {DateOccurred}", domainEvent.Id, domainEvent.DateOccurred);
        return Task.CompletedTask;
    }
}
