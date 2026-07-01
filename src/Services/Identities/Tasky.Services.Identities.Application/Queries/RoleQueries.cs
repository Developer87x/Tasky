using Dapper;
using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public class RoleQueries(string connectionString, ILogger<RoleQueries> logger) : IRoleQueries
{
    private readonly string _connectionString = string.IsNullOrWhiteSpace(connectionString) ? throw new ArgumentNullException(nameof(connectionString)) : connectionString;
    private readonly ILogger<RoleQueries> _logger = logger;

    public Task<RoleDto?> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var sql = @"
            SELECT r.id as roleId, r.role_name AS name,
                p.id as permissionId , p.permission_name AS name
            FROM identities.roles r
            LEFT JOIN identities.role_permissions rp ON r.id = rp.role_id
            LEFT JOIN identities.permissions p ON rp.permission_id = p.id
            WHERE r.id = @RoleId;
        ";
        _logger.LogInformation("Executing SQL query to retrieve role by ID: {RoleId}", roleId);
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        var roleDictionary = new Dictionary<Guid, RoleDto>();
        var result = connection.Query<RoleDto, PermissionDto, RoleDto>(
            sql,
            (role, permission) =>
            {
                if (!roleDictionary.TryGetValue(role.RoleId, out var roleEntry))
                {
                    roleEntry = role;
                    roleEntry.Permissions = new List<PermissionDto>();
                    roleDictionary.Add(roleEntry.RoleId, roleEntry);       
                }

                if (permission != null && !roleEntry.Permissions.Any(p => p.PermissionId == permission.PermissionId))
                {
                    roleEntry.Permissions.Add(permission);
                }

                return roleEntry;
            },
            new { RoleId = roleId },
            splitOn: "permissionId"
        );

        return Task.FromResult(roleDictionary.Values.FirstOrDefault());
    }

    public Task<PaginatedResult<RoleDto>> GetRolesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        using var connection = new Npgsql.NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT r.id as roleId, r.role_name AS name,
                p.id as permissionId , p.permission_name AS name
            FROM identities.roles r
            LEFT JOIN identities.role_permissions rp ON r.id = rp.role_id
            LEFT JOIN identities.permissions p ON rp.permission_id = p.id
            ORDER BY r.role_name
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
        ";

        var roleDictionary = new Dictionary<Guid, RoleDto>();
        _logger.LogInformation("Executing SQL query to retrieve roles with pagination: PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
        var result = connection.Query<RoleDto, PermissionDto, RoleDto>(
            sql,
            (role, permission) =>
            {
                if (!roleDictionary.TryGetValue(role.RoleId, out var roleEntry))
                {
                    roleEntry = role;
                    roleEntry.Permissions = [];
                    roleDictionary.Add(roleEntry.RoleId, roleEntry);
                }

                if (permission != null && !roleEntry.Permissions.Any(p => p.PermissionId == permission.PermissionId))
                {
                    roleEntry.Permissions.Add(permission);
                }

                return roleEntry;
            },
            new { Offset = (pageNumber - 1) * pageSize, PageSize = pageSize },
            splitOn: "permissionId"
        );

        var totalCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM identities.roles;");
        _logger.LogInformation("Retrieved {Count} roles from the database.", result.Count());

        return Task.FromResult(new PaginatedResult<RoleDto>
        {
            Items = [.. result.Distinct()],
            TotalCount = totalCount,
            PageSize = pageSize,
            CurrentPage = pageNumber
        });
    
    }
}