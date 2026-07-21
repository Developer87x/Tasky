using Tasky.Services.Projects.Domain.SharedKernel;

namespace Tasky.Services.Projects.Domain.Repositories;

public interface IRepository<TC> where TC : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
