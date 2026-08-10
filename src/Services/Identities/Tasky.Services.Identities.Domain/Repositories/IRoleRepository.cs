using Tasky.BuildingBlocks.Core.EfCore;
using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Domain.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    new Task<Role> AddAsync(Role role, CancellationToken cancellationToken = default);
    new Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    new Task UpdateAsync(Role role, CancellationToken cancellationToken = default);
}