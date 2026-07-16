namespace Tasky.Services.Projects.Domain.SharedKernel;

public abstract  class Entity<TKey> :IEntity<TKey>
{
    public  TKey Id { get; } = default!;

    public DateTime CreatedAt => DateTime.UtcNow;

    public DateTime? UpdatedAt {get; protected set;} 

    private Entity(){}
    protected  Entity(TKey id) :this()
    {
        Id = id;
    }
}