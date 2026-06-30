using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.SharedKernel;
using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class UserCreatedEvent : IDomainEvent
{
    public UserCreatedEvent(UserId userId, string? userName, Email? email)
    {
        UserId = userId;
        UserName = userName;
        Email = email;
    }
    public UserId UserId { get; set;}
    public string? UserName { get; set; }
    public Email? Email { get; set; }
    public DateTime DateOccurred => DateTime.UtcNow;
}
public class RoleCreatedDomainEvent(RoleId roleId, string? roleName) : IDomainEvent
{
    public RoleId RoleId { get; set; } = roleId;
    public string? RoleName { get; set; } = roleName;
    public DateTime DateOccurred => DateTime.UtcNow;
}