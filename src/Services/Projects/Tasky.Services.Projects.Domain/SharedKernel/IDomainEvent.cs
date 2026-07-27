namespace Tasky.Services.Projects.Domain.SharedKernel;

public interface IDomainEvent
{
    Guid Id { get; }
    
}

public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}