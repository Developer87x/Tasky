using Tasky.BuildingBlocks.Core.Models;

namespace Tasky.BuildingBlocks.Core.EfCore
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    }
}