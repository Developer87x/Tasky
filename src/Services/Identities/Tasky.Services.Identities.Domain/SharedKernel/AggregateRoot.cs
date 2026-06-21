namespace Tasky.Services.Identities.Domain.SharedKernel;
public class AggregateRoot<TA, TK> : Entity<TK>, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    protected AggregateRoot(TK id) : base(id)
    {
    }

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}