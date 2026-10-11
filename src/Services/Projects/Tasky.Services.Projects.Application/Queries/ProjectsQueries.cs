using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Logging;
using Tasky.Services.Projects.Application.Dtos;

namespace Tasky.Services.Projects.Application.Queries;

public class ProjectsQueries(string connectionString,ILogger<ProjectsQueries> logger) :IProjectsQueries
{
    private readonly string _connectionString = connectionString;
    private ILogger<ProjectsQueries> _logger = logger;
    public async Task<List<ProjectDto>> GetProjects()
    {
        await using var connection = new SqlConnection(_connectionString);
        const string sql = """
                           select * from Projects.projects
                           """;
     var result = await connection.QueryAsync<ProjectDto>(sql);
     return [.. result];
    }

    public Task<ProjectDto?> GetProjectById(Guid id)
    {
        throw new NotImplementedException();
    }
}