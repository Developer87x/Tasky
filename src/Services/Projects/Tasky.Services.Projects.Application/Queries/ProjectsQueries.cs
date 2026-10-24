using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Tasky.Services.Projects.Application.Dtos;

namespace Tasky.Services.Projects.Application.Queries;

public class ProjectsQueries(string connectionString, ILogger<ProjectsQueries> logger) : IProjectsQueries
{
    private readonly string _connectionString = connectionString;
    private readonly ILogger<ProjectsQueries> _logger = logger;

    public async Task<List<ProjectDto>> GetProjects()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        const string sql = """
                           select * from Projects.projects
                           """;
        var result = await connection.QueryAsync<ProjectDto>(sql);
        return [.. result];
    }

    public async Task<ProjectDto?> GetProjectById(Guid id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        const string sql = """
                           select * from Projects.projects
                           join Projects.categories on Projects.projects."CategoryId" = Projects.categories.id
                           where projects.projects.id = @id
                           """;
       
        _logger.LogDebug("Getting project with id {Id}", id);

        var result = await connection.QueryAsync<ProjectDto, CategoryDto, ProjectDto>(
            sql, (project, category) =>
            {
                project.Category = category;
                return project;
            },
            new { id },
            splitOn: "CategoryId");

        _logger.LogDebug("Project with id {Id} found", id);
        return result.FirstOrDefault();
    }
}