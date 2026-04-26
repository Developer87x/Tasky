using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Domain.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<Role> AddAsync(Role role);
    }
}