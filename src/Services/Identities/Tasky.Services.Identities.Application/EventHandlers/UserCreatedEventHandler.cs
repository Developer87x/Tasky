using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Application.EventHandlers;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger) : IDomainEventHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger = logger;
    public Task Handle(UserCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("UserCreatedEvent handled for UserId: {UserId}, UserName: {UserName}, Email: {Email}", domainEvent.UserId, domainEvent.UserName, domainEvent.Email);
        return Task.CompletedTask;
    }
}
