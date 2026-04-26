namespace Tasky.Services.Identities.Domain.Entities
{
    public class Role
    {
        private readonly List<User> _users = [];
        public Guid Id { get; private set; }
        public string? RoleName { get; private set; }

        public IReadOnlyCollection<User> Users => _users.AsReadOnly();  
        private Role() { }
        private Role(Guid id, string? roleName) : this()
        {
            Id = id;
            RoleName = roleName;
        }
        public static Role Create(string? roleName) => new(Guid.NewGuid(), roleName);
        public void UpdateRoleName(string? roleName) => RoleName = roleName;
    }
}