using Tasky.BuildingBlocks.Core.EfCore;
using Tasky.Services.Projects.Domain.Entities;

namespace Tasky.Services.Projects.Domain.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    new Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    new Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<Category?> GetByNameAsync(string categoryName, CancellationToken cancellationToken = default);
}