namespace Tasky.Services.Projects.Domain.SharedKernel;

public class AggregateRoot<TClass, TKey> : Entity<TKey>, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    protected AggregateRoot(TKey id) : base(id)
    {
    }

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}

