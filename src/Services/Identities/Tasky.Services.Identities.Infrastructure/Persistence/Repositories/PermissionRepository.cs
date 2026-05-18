using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Infrastructure.Persistence.Repositories;

public class PermissionRepository(IdentityDb db) : IPermissionRepository
{
    private readonly IdentityDb _db = db;

    public IUnitOfWork UnitOfWork => _db;

    public async Task<Permission> AddAsync(Permission permission)
    {
        var entry = await _db.Permissions.AddAsync(permission);
        return entry.Entity;
    }

    public async Task<Permission?> GetByNameAsync(string permissionName)
    {
        return await _db.Permissions.FirstOrDefaultAsync(p => p.PermissionName == permissionName);
    }
}
