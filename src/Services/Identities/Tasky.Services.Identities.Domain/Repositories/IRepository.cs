namespace Tasky.Services.Identities.Domain.Repositories;

public interface IRepository<TC> where TC : class
{   
    IUnitOfWork UnitOfWork { get; }
}
