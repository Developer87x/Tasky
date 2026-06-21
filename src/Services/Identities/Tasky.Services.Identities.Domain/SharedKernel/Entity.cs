namespace Tasky.Services.Identities.Domain.SharedKernel;

public class Entity<T> : IEntity<T>
{

    public T Id { get; protected set; } =default!;
    private Entity() { }
    protected Entity(T id) : this() => Id = id;

}

