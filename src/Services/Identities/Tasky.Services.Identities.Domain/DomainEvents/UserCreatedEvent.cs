using Tasky.BuildingBlocks.Core.Events;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class UserCreatedEvent(UserId userId,  Email? email) : IDomainEvent
{
    public UserId UserId { get; set; } = userId;
    public Email? Email { get; set; } = email;

    public Guid EventId {get;set;}= Guid.NewGuid();

    public DateTime? OccurredOn {get;set;}= DateTime.UtcNow;
}
