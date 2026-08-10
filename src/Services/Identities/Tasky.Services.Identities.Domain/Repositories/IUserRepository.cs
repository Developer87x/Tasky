using Microsoft.EntityFrameworkCore;
using Tasky.BuildingBlocks.Core.EfCore;
using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Domain.Repositories;

public interface IUserRepository:IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
}

public interface IIdentityDbContext: IDbaseContext
{
    DbSet<User> Users { get; set; }      
    DbSet<Role> Roles { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<Permission> Permissions { get; set; }
}