using Microsoft.Extensions.Logging;
using Tasky.BuildingBlocks.Core.Events;
using Tasky.Services.Identities.Domain.DomainEvents;

namespace Tasky.Services.Identities.Application.EventHandlers;

public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger) : IDomainEventHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger = logger;
  
    public Task HandleAsync(UserCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("UserCreatedEvent handled for UserId: {UserId},  Email: {Email}", domainEvent.UserId,  domainEvent.Email);
        return Task.CompletedTask;
    }   
    
}
