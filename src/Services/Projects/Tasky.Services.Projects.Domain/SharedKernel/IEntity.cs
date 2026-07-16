namespace Tasky.Services.Projects.Domain.SharedKernel;

public interface IEntity<Tkey>
{
    public  Tkey Id { get; }
    public DateTime CreatedAt { get;  }
    public DateTime? UpdatedAt { get;  }
}