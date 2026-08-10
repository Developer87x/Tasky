using Microsoft.Extensions.Logging;
using Tasky.BuildingBlocks.Core.EfCore;
using Tasky.Services.Projects.Domain.Entities;
using Tasky.Services.Projects.Domain.Repositories;

namespace Tasky.Services.Projects.Infrastructure.Persistence.Repositories;

public class ProjectRepository(ProjectDb dbContext) : IProjectRepository
{
    private readonly ProjectDb _dbContext = dbContext;
    public IUnitOfWork UnitOfWork => _dbContext;
    public Task<Project> AddAsync(Project entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Project> UpdateAsync(Project entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}