using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Domain.Repositories
{
    public interface IUserRepository:IRepository<User>
    {
        Task<User> AddAsync(User user);
        Task<User?> GetByUserNameAsync(string userName);
        Task<User?> GetByIdAsync(Guid id);
        Task<bool> IsEmailExistsAsync(string email);
    }
}
