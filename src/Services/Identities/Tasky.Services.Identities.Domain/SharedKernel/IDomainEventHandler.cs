namespace Tasky.Services.Identities.Domain.SharedKernel;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T domainEvent);
}

