using Tasky.Services.Projects.Domain.SharedKernel;

namespace Tasky.Services.Projects.Domain.DomainEvents;

public class CategoryCreatedEvent(Guid id) : IDomainEvent
{
    public Guid Id { get; } = id;
}

