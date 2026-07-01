using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.SharedKernel;
using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class UserCreatedEvent(UserId userId, string? userName, Email? email) : IDomainEvent
{
    public UserId UserId { get; set; } = userId;
    public string? UserName { get; set; } = userName;
    public Email? Email { get; set; } = email;
    public DateTime DateOccurred => DateTime.UtcNow;
}
