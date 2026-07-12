using Tasky.Services.Projects.Domain.SharedKernel;

namespace Tasky.Services.Projects.Domain.DomainEvents;

public class ProjectCreatedEvent:IDomainEvent
{
    public Guid Id { get; }

    public ProjectCreatedEvent(Guid id)
    {
        Id = id;
    }
}