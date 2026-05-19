using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class PermissionCreatedDomainEvent(PermissionId id) : IDomainEvent
{
    public PermissionId Id { get; set; } = id;
    public DateTime DateOccurred { get; private set; } = DateTime.UtcNow;
    public int Version => throw new NotImplementedException();
}
