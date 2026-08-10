using Tasky.BuildingBlocks.Core.Events; 

namespace Tasky.Services.Projects.Domain.DomainEvents;

public class ProjectManagerAssignedEvent(Guid projectId, string newProjectManagerId) : IDomainEvent
{
    public Guid ProjectId { get; } = projectId;
    public string NewProjectManagerId { get; } = newProjectManagerId;
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime? OccurredOn { get; } = DateTime.UtcNow;
}