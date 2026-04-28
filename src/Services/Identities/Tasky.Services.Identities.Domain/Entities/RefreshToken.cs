using System.Security.Cryptography;
using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.Entities;

public class RefreshToken : AggregateRoot<RefreshToken, RefreshTokenId>
{
    public string? Token { get; private set; }
    public DateTime Expires { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime CreatedAt { get; private set; }
    public UserId UserId { get; private set; }
    public User? User { get; private set; }
    public bool IsRevoked { get; private set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public string? RawToken { get; private set; }

    private RefreshToken() : base(RefreshTokenId.NewId()) { }

    private RefreshToken(RefreshTokenId id, string? token, DateTime expires, DateTime createdAt, UserId userId) : this()
    {
        Id = id;
        Token = token;
        Expires = expires;
        CreatedAt = createdAt;
        UserId = userId;
        IsRevoked = false;
    }


    public static RefreshToken Create(UserId userId)
    {
        var rowToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var tokenHased = Convert.ToBase64String(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rowToken)));
        var refreshToken = new RefreshToken(RefreshTokenId.NewId(), tokenHased, DateTime.UtcNow.AddDays(7), DateTime.UtcNow, userId)
        {
            RawToken = rowToken
        };
        return refreshToken;
    }

    public bool IsActive => !IsExpired && !IsRevoked;
    public void Revoke() => IsRevoked = true;
}