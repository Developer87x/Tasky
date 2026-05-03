using Microsoft.EntityFrameworkCore;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;
using Tasky.Services.Identities.Domain.SharedKernel;
using Tasky.Services.Identities.Infrastructure.Persistence.EntitiesConfigurations;

namespace Tasky.Services.Identities.Infrastructure.Persistence;

public class IdentityDb(DbContextOptions<IdentityDb> options,IServiceProvider serviceProvider) : DbContext(options), IUnitOfWork
{
public const string DEFAULT_SCHEMA = "identities";
public DbSet<User> Users { get; set; }
public DbSet<Role> Roles { get; set; }
public DbSet<RefreshToken> RefreshTokens { get; set; }
public DbSet<Permission> Permissions { get; set; }

    public  async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {

        //get all domain events from the tracked entities
        var domainEntities = ChangeTracker.Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();
        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handler = serviceProvider.GetService(handlerType);
            if (handler is not null)
                await (Task)handlerType.GetMethod("Handle")!.Invoke(handler, [domainEvent])!;
            
        }

        var result = await  base.SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new RoleEntityConfiguration()); 
        modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionEntityConfiguration());
        
    }
}