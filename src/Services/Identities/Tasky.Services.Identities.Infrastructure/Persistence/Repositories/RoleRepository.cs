using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Infrastructure.Persistence.Repositories;

public class RoleRepository(IdentityDb db) : IRoleRepository
{

    private readonly IdentityDb _db = db;

    public IUnitOfWork UnitOfWork => _db; 

    public async Task<Role> AddAsync(Role role)
    {
        var entry = await _db.Roles.AddAsync(role);
        return entry.Entity;
    }

    public Task<Role?> GetByIdAsync(Guid id)
    {
        var role = _db.Roles.FirstOrDefaultAsync(r => r.Id.Value == id);
        return role;
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleName == name);
        return role;
    }
}