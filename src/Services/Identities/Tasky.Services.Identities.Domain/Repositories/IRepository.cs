using Tasky.Services.Identities.Domain.SharedKernel;

namespace Tasky.Services.Identities.Domain.Repositories;

public interface IRepository<TC> where TC : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
