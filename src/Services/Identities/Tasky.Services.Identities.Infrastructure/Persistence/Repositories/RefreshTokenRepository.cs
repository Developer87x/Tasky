using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(IdentityDb db) : IRefreshTokenRepository
{
    public IUnitOfWork UnitOfWork => db;

    public async Task<RefreshToken> GetByTokenAsync(string rawToken)
    {
        var tokenHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawToken)));
        var result = await db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == tokenHash);
        return result!;
    }
}