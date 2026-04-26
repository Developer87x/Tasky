using System.Security.Cryptography;

namespace Tasky.Services.Identities.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }
        public string? Token { get; private set; }
        public DateTime Expires { get; private set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime CreatedAt { get; private set; }
        public Guid UserId { get; private set; }
        public User? User { get; private set; }
        public bool IsRevoked { get; private set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string? RawToken { get; private set; }
        private RefreshToken() { }


        public static RefreshToken Create(Guid userId)
        {
            var rowToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var tokenHased = Convert.ToBase64String(SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rowToken)));
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = tokenHased,
                RawToken = rowToken,
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };
            return refreshToken;
        }
    

        public bool IsActive => !IsExpired && !IsRevoked;
        public void Revoke() => IsRevoked = true;


    }
}