using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Domain.SharedKernel;

public class Entity<T> : IEntity<T>
{

    public T Id { get; protected set; } =default!;
    protected Entity() { }
    protected Entity(T id) : this() => Id = id;

}

