namespace Tasky.Services.Projects.Domain.SharedKernel;

public interface IEntity<out Tkey>
{
    public  Tkey Id { get; }
}