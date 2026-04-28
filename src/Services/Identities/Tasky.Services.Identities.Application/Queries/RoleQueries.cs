using Dapper;
using Npgsql;
using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public class RoleQueries(string connectionString) : IRoleQueries
{
    private readonly string _connectionString = string.IsNullOrWhiteSpace(connectionString) ? throw new ArgumentNullException(nameof(connectionString)) : connectionString;

    public async Task<List<RoleDto>> GetAllRolesAsync()
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var roleList = await connection.QueryAsync<RoleDto>("SELECT role_name as RoleName FROM identities.roles");
        return [.. roleList];
    }

    public async Task<PaginationDto<RoleDto>> GetAllRolesAsync(PaginationRequestDto paginationRequest)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var totalCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM identities.roles");
        var roles = await connection.QueryAsync<RoleDto>("SELECT role_name as RoleName FROM identities.roles ORDER BY role_name OFFSET @Offset LIMIT @PageSize", new { Offset = paginationRequest.Offset, PageSize = paginationRequest.PageSize });

        return new PaginationDto<RoleDto>
        {
            Items = [.. roles],
            TotalCount = totalCount,
            Page = paginationRequest.Page,
            PageSize = paginationRequest.PageSize
        };
    }

    public async Task<RoleDto> GetRoleByNameAsync(string roleName)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        var role = await connection.QueryFirstOrDefaultAsync<RoleDto>("SELECT  role_name as RoleName FROM identities.roles WHERE role_name = @RoleName", new { RoleName = roleName });
        return role!;
    }

}