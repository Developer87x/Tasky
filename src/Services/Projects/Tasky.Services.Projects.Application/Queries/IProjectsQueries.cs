using Tasky.Services.Projects.Application.Dtos;

namespace Tasky.Services.Projects.Application.Queries;

public interface IProjectsQueries 
{
    Task<List<ProjectDto>> GetProjects();
    Task<ProjectDto?> GetProjectById(Guid id);
}