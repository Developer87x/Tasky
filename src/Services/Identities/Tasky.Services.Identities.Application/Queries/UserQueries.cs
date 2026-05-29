using Dapper;
using Npgsql;
using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public class UserQueries(string connectionString) : IUserQueries
{

    private readonly string _connectionString = string.IsNullOrWhiteSpace(connectionString) ? throw new ArgumentNullException(nameof(connectionString)) : connectionString;

    public async Task<Pagination<UserDto>> GetAllUserAsync(PaginationRequest paginationRequest)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var totalCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM identities.users");
        var userDict = new Dictionary<string, UserDto>();
        var roleDict = new Dictionary<string, RoleDto>(); // key: "email|roleName" to scope roles per user

        await connection.QueryAsync<UserDto, RoleDto, PermissionDto, UserDto>(
            @"SELECT usr.email, usr.username AS UserName, usr.is_active AS IsActive,
                r.role_name AS RoleName,
                p.permission_name AS PermissionName
                FROM (SELECT id, email, username,is_active FROM identities.users LIMIT @PageSize OFFSET @Offset) usr
                JOIN identities.user_roles ur ON ur.user_id = usr.id
                JOIN identities.roles r ON r.id = ur.role_id
                LEFT JOIN identities.role_permissions rp ON rp.role_id = r.id
                LEFT JOIN identities.permissions p ON p.id = rp.permission_id",
            (user, role, permission) =>
            {
                if (!userDict.TryGetValue(user.Email!, out var existingUser))
                {
                    existingUser = user;
                    existingUser.Roles = [];
                    userDict[user.Email!] = existingUser;
                }
                if (role != null)
                {
                    var roleKey = $"{user.Email}|{role.RoleName}";
                    if (!roleDict.TryGetValue(roleKey, out var existingRole))
                    {
                        existingRole = role;
                        existingRole.Permissions = [];
                        roleDict[roleKey] = existingRole;
                        existingUser.Roles!.Add(existingRole);
                    }
                    if (permission != null)
                        existingRole.Permissions!.Add(permission);
                }
                return existingUser;
            },
            new { Offset = paginationRequest.Offset, PageSize = paginationRequest.PageSize },
            splitOn: "RoleName,PermissionName");

        return new()
        {
            Items = [.. userDict.Values],
            TotalCount = totalCount,
            Page = paginationRequest.Page,
            PageSize = paginationRequest.PageSize
        };
    }

    public async Task<UserDto> GetUserById(Guid id)
    {

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var command = await connection.QueryAsync<UserDto, RoleDto, PermissionDto, UserDto>(
            @"SELECT usr.email, usr.username AS UserName,
                r.role_name AS RoleName,
                p.permission_name AS PermissionName
                FROM identities.users usr
                JOIN identities.user_roles ur ON ur.user_id = usr.id
                JOIN identities.roles r ON r.id = ur.role_id
                LEFT JOIN identities.role_permissions rp ON rp.role_id = r.id
                LEFT JOIN identities.permissions p ON p.id = rp.permission_id
                WHERE usr.id = @Id",
            (user, role, permission) =>
            {
                user.Roles = role == null ? [] : [role];
                role?.Permissions = permission == null ? [] : [permission];
                return user;
            },
            new { Id = id },
            splitOn: "RoleName,PermissionName");
        return command.FirstOrDefault()!;
    }


}