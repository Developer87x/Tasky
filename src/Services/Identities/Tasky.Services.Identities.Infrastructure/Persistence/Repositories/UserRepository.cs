using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Infrastructure.Persistence.Repositories;

public class UserRepository(IdentityDb db) : IUserRepository
{

    private readonly IdentityDb _db = db;

    public IUnitOfWork UnitOfWork => _db;

    public async Task<User> AddAsync(User user)
    {
        var entry = await _db.Users.AddAsync(user);
        return entry.Entity;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var userId = UserId.From(id);
        return await _db.Users.
        Include(s => s.RefreshTokens).
        Include(s => s.Roles).
        FirstOrDefaultAsync(u => u.Id.Equals(userId));
    }
    public async Task<User?> GetByUserNameAsync(string userName)
    {
        return await _db.Users.
        Include(s => s.RefreshTokens).
        Include(s => s.Roles).
            ThenInclude(r => r.Permissions).
        FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public Task<bool> IsEmailExistsAsync(string email)
    {
        return _db.Users.AnyAsync(u => u.Email!.Value == email);

    }

    public Task<bool> IsUserNameExistsAsync(string userName)
    {
        return _db.Users.AnyAsync(u => u.UserName == userName);
    }
}
