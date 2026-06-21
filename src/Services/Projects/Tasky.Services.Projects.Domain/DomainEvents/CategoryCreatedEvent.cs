using Tasky.Services.Projects.Domain.SharedKernel;

namespace Tasky.Services.Projects.Domain.DomainEvents;

public class CategoryCreatedEvent:IDomainEvent
{
    public Guid Id { get; }

    public CategoryCreatedEvent(Guid id)
    {
        Id = id;
    }
}