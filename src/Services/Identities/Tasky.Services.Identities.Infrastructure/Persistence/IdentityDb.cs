using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;
using Tasky.Services.Identities.Infrastructure.Persistence.EntitiesConfigurations;

namespace Tasky.Services.Identities.Infrastructure.Persistence;

public class IdentityDb(DbContextOptions<IdentityDb> options) : DbContext(options), IUnitOfWork
{

public const string DEFAULT_SCHEMA = "identities";
public DbSet<User> Users { get; set; }
public DbSet<Role> Roles { get; set; }
public DbSet<RefreshToken> RefreshTokens { get; set; }

    public  async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        var result = await  base.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RoleEntityConfiguration()); 
        modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfiguration());
    }
}