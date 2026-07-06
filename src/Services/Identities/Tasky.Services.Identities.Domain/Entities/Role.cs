using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.Entities;

public class Role :AggregateRoot<Role, RoleId>
{
    private readonly List<Permission> _permissions = [];
    private readonly List<User> _users = [];
    public string? RoleName { get; private set; }

    public IReadOnlyCollection<User> Users => _users.AsReadOnly();  
    public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();
    private Role() :base(RoleId.NewId()) { }
    private Role(RoleId id, string? roleName) :this()
    {
        Id = id;
        RoleName = roleName;
        AddDomainEvent(new RoleCreatedDomainEvent(Id, roleName));
    }
    public static Role Create(string? roleName) => new(RoleId.NewId(), roleName);
    public void UpdateRoleName(string? roleName) => RoleName = roleName;
    public void AssignPermissionToRole(Permission permission)
    {
        if (_permissions.Any(p => p.Id == permission.Id))
            throw new DomainException($"Permission with Id {permission.Id} is already assigned to the role.");
        _permissions.Add(permission);
    }
}
