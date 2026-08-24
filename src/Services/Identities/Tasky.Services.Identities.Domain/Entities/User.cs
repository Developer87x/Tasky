using Tasky.BuildingBlocks.Core.Exceptions;
using Tasky.BuildingBlocks.Core.Models;
using Tasky.Services.Identities.Domain.DomainEvents;
using Tasky.Services.Identities.Domain.ValueObjects;
namespace Tasky.Services.Identities.Domain.Entities;

public class User : AggregateRoot<UserId>
{

    private readonly List<Role> _roles = [];
    private readonly List<RefreshToken> _refreshTokens = [];

    public Email? Email { get; private set; }
    public string? UserName { get; private set; }
    public Password? Password { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();
    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
    private User() :base(UserId.NewId()) { }

    public User(UserId id, string userName, Email email, Password password,DateTime? lastModified = null,string? lastModifiedBy =null) : base(id)
    {
        
        UserName = userName;
        Email = email;
        Password = password;
        LastModified = lastModified;
        CreatedAt = DateTime.UtcNow;
        CreatedBy= "system";
        IsActive= false;
        LastModifiedBy = lastModifiedBy;
        AddDomainEvent(new UserCreatedEvent(id,email));
    }
    
    public void Activate()
    {
        IsActive = true;
        LastModified = DateTime.UtcNow;
    }
    public void Deactivate()
    {
        IsActive = false;
        LastModified = DateTime.UtcNow;
    }   
    public void UpdateEmail(Email? email)
    {
        Email= email;
        LastModified = DateTime.UtcNow;
    }
    public void AddRole(Role role)
    {
        switch (_roles.Contains(role))
        {
            case false:
                _roles.Add(role);
                LastModified = DateTime.UtcNow;
                return;
            default:
                throw new DomainException("Role already assigned to user.");
        }
    }
    public void UpdatePassword(Password password)
    {
        Password = password;
        LastModified = DateTime.UtcNow;
    }
    public RefreshToken AddRefreshToken()
    {
        foreach (var token in _refreshTokens.Where(token => token.IsActive))
        {
            token.Revoke();
        }
        var newToken = RefreshToken.Create(Id);
        _refreshTokens.Add(newToken);
        return newToken;
    }
    public static User Create(Email email,string userName, Password password,DateTime? lastModified = null,string? lastModifiedBy =null)=> new (UserId.NewId(),userName,email,password,lastModified,lastModifiedBy)
    {
        CreatedAt = DateTime.UtcNow,
        CreatedBy = null,
        IsDeleted = false,
        IsActive = true
    };
}
