using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class ActiviatedUserDomainEvent(UserId id) : IDomainEvent
{
    public UserId Id { get; set; } = id;
    public DateTime DateOccurred { get; private set; } = DateTime.UtcNow;

    public int Version => throw new NotImplementedException();
}