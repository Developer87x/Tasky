namespace Tasky.Services.Projects.Domain.SharedKernel;

public abstract  class Entity<TKey> :IEntity<TKey>
{
    public TKey Id { get; }
    private Entity(){}
    protected  Entity(TKey id) :this()
    {
        Id = id;
    }
}