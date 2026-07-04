using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public class UserQueries(string connectionString, ILogger<UserQueries> logger) : IUserQueries
{
    private readonly string _connectionString = connectionString;
    private readonly ILogger<UserQueries> _logger = logger;

    public async Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        
        using var connection = new NpgsqlConnection(_connectionString);
        const string sql = @"
            SELECT u.id , u.username, u.email, u.created_at as createdAt,u.updated_at as updatedAt,is_active as isActive,
            r.id as roleId, r.role_name AS name,
            p.id as permissionId , p.permission_name AS name
            FROM identities.users u
            LEFT JOIN identities.user_roles ur ON u.id = ur.user_id
            LEFT JOIN identities.roles r ON ur.role_id = r.id
            LEFT JOIN identities.role_permissions rp ON r.id = rp.role_id
            LEFT JOIN identities.permissions p ON rp.permission_id = p.id
            
            WHERE u.id = @UserId;
        ";

        var userDictionary = new Dictionary<Guid, UserDto>();
        _logger.LogInformation("Executing SQL query to retrieve user by ID: {UserId}", userId);
        var result = await connection.QueryAsync<UserDto, RoleDto, PermissionDto, UserDto>(
            sql,
            (user, role, permission) =>
            {
                if (!userDictionary.TryGetValue(user.Id, out var userEntry))
                {
                    userEntry = user;
                    userEntry.Roles = [];
                    userDictionary.Add(userEntry.Id, userEntry);
                }

                if (role != null && !userEntry.Roles.Any(r => r.RoleId == role.RoleId))
                {
                    role.Permissions = [];
                    userEntry.Roles.Add(role);
                }

                if (permission != null && role != null)
                {
                    var roleEntry = userEntry.Roles.First(r => r.RoleId == role.RoleId);
                    if (!roleEntry.Permissions.Any(p => p.PermissionId == permission.PermissionId))
                    {
                        roleEntry.Permissions.Add(permission);
                    }
                }

                return userEntry;
            },
            new { UserId = userId },
            splitOn: "roleId,permissionId"
        );

        return userDictionary.Values.FirstOrDefault();
    }
}