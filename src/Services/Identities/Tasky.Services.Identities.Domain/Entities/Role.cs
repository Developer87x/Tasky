using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.Entities;

public class Role :AggregateRoot<Role, RoleId>
{
    private readonly List<User> _users = [];
    public string? RoleName { get; private set; }

    public IReadOnlyCollection<User> Users => _users.AsReadOnly();  
    private Role() :base(RoleId.NewId()) { }
    private Role(RoleId id, string? roleName) :this()
    {
        Id = id;
        RoleName = roleName;
    }
    public static Role Create(string? roleName) => new(RoleId.NewId(), roleName);
    public void UpdateRoleName(string? roleName) => RoleName = roleName;
}