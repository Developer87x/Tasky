using Tasky.BuildingBlocks.Core.Events;
namespace Tasky.BuildingBlocks.Core.Models
{
    public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
    {
        protected AggregateRoot(TId id)
        {
            Id =id;
        }
        private readonly List<IDomainEvent> _domainEvents = [];
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}