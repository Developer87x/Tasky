using Tasky.Services.Projects.Domain.SharedKernel;

namespace Tasky.Services.Projects.Domain.DomainEvents;

public class ProjectManagerAssignedEvent(Guid projectId, string newProjectManagerId) : IDomainEvent
{
    public Guid ProjectId { get; } = projectId;
    public string NewProjectManagerId { get; } = newProjectManagerId;
}