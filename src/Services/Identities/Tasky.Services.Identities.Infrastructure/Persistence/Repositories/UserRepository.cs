using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Infrastructure.Persistence.Repositories;

public class UserRepository(IdentityDb db) : IUserRepository
{

    private readonly IdentityDb _db = db;

    public IUnitOfWork UnitOfWork => _db;

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        var entry = await _db.Users.AddAsync(user, cancellationToken);
        return entry.Entity;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var userId = UserId.From(id);
        return await _db.Users.
        Include(s => s.RefreshTokens).
        Include(s => s.Roles).
        FirstOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
    }
    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _db.Users.
        Include(s => s.RefreshTokens).
        Include(s => s.Roles).
            ThenInclude(r => r.Permissions).
        FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }

    public Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return _db.Users.AnyAsync(u => u.Email!.Value == email, cancellationToken);

    }

    public Task<bool> IsUserNameExistsAsync(string userName, CancellationToken cancellationToken = default)
    {
        return _db.Users.AnyAsync(u => u.UserName == userName, cancellationToken);
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var updedEnitiy = _db.Users.Update(user);
        return updedEnitiy.Entity;
    }
}
