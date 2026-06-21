namespace Tasky.Services.Projects.Domain.SharedKernel;

public interface IEntity<Tkey>
{
    public  Tkey Id { get; }
}