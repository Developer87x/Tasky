using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class RoleCreatedDomainEvent(RoleId id) : IDomainEvent
{
    public RoleId Id { get; set; } = id;
    public DateTime DateOccurred { get; private set; } = DateTime.UtcNow;

    public int Version => throw new NotImplementedException();
}
