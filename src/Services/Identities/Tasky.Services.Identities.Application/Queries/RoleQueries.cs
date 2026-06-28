namespace Tasky.Services.Identities.Application.Queries;

public class RoleQueries(string connectionString) : IRoleQueries
{
    private readonly string _connectionString = string.IsNullOrWhiteSpace(connectionString) ? throw new ArgumentNullException(nameof(connectionString)) : connectionString;

}