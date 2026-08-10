using Tasky.BuildingBlocks.Core.Events;

namespace Tasky.Services.Projects.Domain.DomainEvents;

public class ProjectCreatedEvent(Guid id) : IDomainEvent
{
    public Guid Id { get; } = id;
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime? OccurredOn { get; } = DateTime.UtcNow;
}
