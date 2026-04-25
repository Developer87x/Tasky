using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Domain.Repositories;

public interface IRefreshTokenRepository :IRepository<RefreshToken>
{
    Task<RefreshToken> GetByTokenAsync(string token);
}