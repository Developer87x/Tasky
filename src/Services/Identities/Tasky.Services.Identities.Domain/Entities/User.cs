using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.SharedKernel;
using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Domain.Entities;

public class User : AggregateRoot<User, UserId>
{

    private readonly List<Role> _roles = [];
    private readonly List<RefreshToken> _refreshTokens = [];

    public Email? Email { get; private set; }
    public string? UserName { get; private set; }
    public Password? Password { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    private User() :base(UserId.NewId()) { }
    private User(UserId id, Email? email, string? userName, Password? password, DateTime createdAt, DateTime? updatedAt) : this()
    {
        Id = id;
        Email = email;
        UserName = userName;
        Password = password;
        IsActive = false;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        AddDomainEvent(new UserCreatedEvent(id, userName, email));
    }

    public static User Create(Email email, string? userName, Password password) => new(UserId.NewId(), email, userName, password, DateTime.UtcNow, null);

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

    public void UpdateEmail(Email? email)
    {
        Email= email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddRole(Role role)
    {
        if (!_roles.Contains(role))
        {
            _roles.Add(role);
            UpdatedAt = DateTime.UtcNow;
            return;
        }
        throw new DomainException("Role already assigned to user.");
    }


    public void UpdatePassword(Password password)
    {
        Password = password;
        UpdatedAt = DateTime.UtcNow;
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
