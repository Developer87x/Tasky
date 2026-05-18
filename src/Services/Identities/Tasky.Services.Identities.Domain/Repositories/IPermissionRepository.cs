using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Domain.Repositories;

public interface IPermissionRepository :IRepository<Permission>
{
    Task<Permission?> GetByNameAsync(string permissionName);
    Task<Permission> AddAsync(Permission permission);
}