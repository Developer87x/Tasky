using Tasky.BuildingBlocks.Core.EfCore;
using Tasky.Services.Projects.Domain.Entities;

namespace Tasky.Services.Projects.Domain.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetByProjectCodeAsync(string projectCode, CancellationToken cancellationToken = default);
}