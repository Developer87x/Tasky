using Microsoft.EntityFrameworkCore;
using Tasky.BuildingBlocks.Core.EfCore;
using Tasky.Services.Projects.Domain.Entities;
using Tasky.Services.Projects.Domain.Repositories;

namespace Tasky.Services.Projects.Infrastructure.Persistence.Repositories;

public class CategoryRepository(ProjectDb dbContext) : ICategoryRepository
{
    private readonly ProjectDb _dbContext = dbContext;
    public IUnitOfWork UnitOfWork => _dbContext;

    public Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        _dbContext.Categories.Add(category);
        return Task.FromResult(category);
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if(id == Guid.Empty)
        {
            throw new ArgumentException("Category ID cannot be empty.", nameof(id));
        }
        var categoryId = CategoryId.From(id);
        return _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId, cancellationToken);
    }

    public Task<Category> UpdateAsync(Category entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Category?> GetByNameAsync(string categoryName, CancellationToken cancellationToken = default)
    {
        return _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName, cancellationToken);
    }
}