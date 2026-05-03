using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.Entities;

public class Permission :AggregateRoot<Permission, PermissionId>
{
    private readonly List<Role> _roles = [];
    public string? PermissionName { get; private set; }

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();  
    private Permission() :base(PermissionId.NewId()) { }
    private Permission(PermissionId id, string? permissionName) :this()
    {
        Id = id;
        PermissionName = permissionName;
    }
    public static Permission Create(string? permissionName) => new(PermissionId.NewId(), permissionName);
}