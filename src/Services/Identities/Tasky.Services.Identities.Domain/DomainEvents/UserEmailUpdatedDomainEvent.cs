using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class UserEmailUpdatedDomainEvent(UserId id) : IDomainEvent
{
    public UserId Id { get; set; } = id;
    public DateTime DateOccurred => DateTime.UtcNow;

    public int Version => throw new NotImplementedException();
}