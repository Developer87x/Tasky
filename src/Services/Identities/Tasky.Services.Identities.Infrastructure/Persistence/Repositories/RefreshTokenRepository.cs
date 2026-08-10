using Microsoft.EntityFrameworkCore;
using Tasky.BuildingBlocks.Core.EfCore;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository(IdentityDb db) : IRefreshTokenRepository
{
    public Task<RefreshToken> AddAsync(RefreshToken entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken> UpdateAsync(RefreshToken entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<RefreshToken> GetByTokenAsync(string rawToken)
    {
        var tokenHash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(rawToken)));
        var result = await db.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == tokenHash);
        return result!;
    }

    IUnitOfWork IRepository<RefreshToken>.UnitOfWork => db;
}