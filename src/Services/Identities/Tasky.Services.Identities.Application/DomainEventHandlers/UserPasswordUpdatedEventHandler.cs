using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Application.DomainEventHandlers;

public class UserPasswordUpdatedEventHandler(ILogger<UserPasswordUpdatedEventHandler> logger) : IDomainEventHandler<UserPasswordUpdatedDomainEvent>
{
    private readonly ILogger<UserPasswordUpdatedEventHandler> _logger =logger;
    public Task Handle(UserPasswordUpdatedDomainEvent domainEvent)
    {
        return Task.CompletedTask;
    }
}