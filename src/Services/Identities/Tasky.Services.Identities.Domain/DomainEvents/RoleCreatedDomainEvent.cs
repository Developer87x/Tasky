using Tasky.BuildingBlocks.Core.Events;
using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class RoleCreatedEvent(RoleId roleId, string? roleName) : IDomainEvent
{
    public RoleId RoleId { get; set; } = roleId;
    public string? RoleName { get; set; } = roleName;

    public Guid EventId => Guid.NewGuid();

    public DateTime? OccurredOn {get;set;}= DateTime.UtcNow;
}