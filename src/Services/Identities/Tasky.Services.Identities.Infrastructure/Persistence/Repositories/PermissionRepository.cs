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

    public Task<List<Permission>> GetPermissionsByIdsAsync(List<Guid> permissionIds, CancellationToken cancellationToken = default)
    {
        var list = new List<PermissionId>();
        foreach (var id in permissionIds)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Permission ID cannot be empty.", nameof(permissionIds));
            }
            var permissionId = PermissionId.From(id);
            list.Add(permissionId);
        }

        return _db.Permissions.Where(p => list.Contains(p.Id)).ToListAsync(cancellationToken);
    }
}
