using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Domain.Entities
{
    public class User
    {

        private readonly List<Role> _roles = [];
        private readonly List<RefreshToken> _refreshTokens = [];
        public Guid Id { get; private set; }
        public string? Email { get; private set; }
        public string? UserName { get; private set; }
        public Password? Password { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
        private User() { }
        private User(Guid id, string? email, string? userName, Password? password, DateTime createdAt, DateTime? updatedAt):this()
        {
            Id = id;
            Email = email;
            UserName = userName;
            Password = password;
            IsActive = false;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public static User Create(string? email, string? userName, Password? password) => new(Guid.NewGuid(), email, userName, password, DateTime.UtcNow, null);

        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateEmail(string? email)
        {
            Email= email;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddRole(Role role)
        {
            if (!_roles.Any(r => r.Id == role.Id))
            {
                _roles.Add(role);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public RefreshToken AddRefreshToken()
        {
            foreach (var token in _refreshTokens)
            {
                if (token.IsActive)
                {
                    token.Revoke();
                }
            }
            var newToken = RefreshToken.Create(Id);
            _refreshTokens.Add(newToken);
            return newToken;
        }
    }
}