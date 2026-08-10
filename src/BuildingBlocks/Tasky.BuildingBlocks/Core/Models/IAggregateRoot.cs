using Tasky.BuildingBlocks.Core.Events;
namespace Tasky.BuildingBlocks.Core.Models
{

    public interface IAggregateRoot
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }

    public interface IAggregateRoot<TId> : IEntity<TId>, IAggregateRoot
    {
    }
}