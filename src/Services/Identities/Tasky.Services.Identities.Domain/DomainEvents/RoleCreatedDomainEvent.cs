using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.DomainEvents;

public class RoleCreatedDomainEvent(RoleId roleId, string? roleName) : IDomainEvent
{
    public RoleId RoleId { get; set; } = roleId;
    public string? RoleName { get; set; } = roleName;
    public DateTime DateOccurred => DateTime.UtcNow;
}