using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Application.DomainEventHandlers;

public class UserEmailUpdatedEventHandler(ILogger<UserEmailUpdatedEventHandler> logger) : IDomainEventHandler<UserEmailUpdatedDomainEvent>
{
    private readonly ILogger<UserEmailUpdatedEventHandler> _logger= logger;
    public Task Handle(UserEmailUpdatedDomainEvent domainEvent)
    {
        _logger.LogInformation("User with id {UserId} updated their email at {DateOccurred}", domainEvent.Id, domainEvent.DateOccurred);
        return Task.CompletedTask;
    }
}
