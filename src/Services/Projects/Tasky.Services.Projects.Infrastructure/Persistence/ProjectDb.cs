using Microsoft.EntityFrameworkCore;
using Tasky.Services.Projects.Domain.Entities;
using Tasky.Services.Projects.Domain.Repositories;
using Tasky.Services.Projects.Domain.SharedKernel;
using Tasky.Services.Projects.Infrastructure.Persistence.EntityConfiigurations;

namespace Tasky.Services.Projects.Infrastructure.Persistence;

public class ProjectDb(DbContextOptions<ProjectDb> options,IServiceProvider serviceProvider) : DbContext(options), IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "projects";
    public DbSet<Category> Categories { get; set; }
    public DbSet<Project> Projects { get; set; }
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        //get all domain events from the tracked entities
        var domainEntities = ChangeTracker.Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();
        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents)
            .ToList();
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handler = serviceProvider.GetService(handlerType);
            if (handler is null)
            {
                continue;
            }

            var handleMethod = handlerType.GetMethod("Handle")
                ?? throw new MissingMethodException(handlerType.FullName, "Handle");

            await (Task)handleMethod.Invoke(handler, [domainEvent, cancellationToken])!;
        }
        return await base.SaveChangesAsync(cancellationToken) > 0;
    }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
    }
}