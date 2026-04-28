namespace Tasky.Services.Identities.Domain.SharedKernel;

public interface IDomainEventHandler<T> where T : IDomainEvent
{
    Task Handle(T domainEvent);
}

