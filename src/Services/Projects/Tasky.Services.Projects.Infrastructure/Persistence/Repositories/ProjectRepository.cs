using Microsoft.EntityFrameworkCore;
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
        _dbContext.Projects.AddAsync(entity, cancellationToken);
        return Task.FromResult(entity);
    }

    public Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var projectId = ProjectId.From(id);
        var project = _dbContext.Projects.FirstOrDefaultAsync(x => x.Id == projectId, cancellationToken);
        return project;
    }

    public Task<Project> UpdateAsync(Project entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Project?> GetByProjectCodeAsync(string projectCode, CancellationToken cancellationToken = default)
    {
        return projectCode switch
        {
            null=> throw new ArgumentException("Project code cannot be empty",nameof(projectCode)),
            _=> _dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectCode == projectCode, cancellationToken)
        };
    }
}