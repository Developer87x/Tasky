using Dapper;
using Npgsql;
using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries
{
    public class UserQueries(string connectionString) : IUserQueries
    {

        private readonly string _connectionString = string.IsNullOrWhiteSpace(connectionString) ? throw new ArgumentNullException(nameof(connectionString)) : connectionString;

        public async Task<PaginationDto<UserDto>> GetAllUserAsync(PaginationRequestDto paginationRequest)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            var totalCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM identities.users");
            var userDict = new Dictionary<string, UserDto>();
            await connection.QueryAsync<UserDto, RoleDto, UserDto>(
                "SELECT usr.email, usr.username as UserName, r.role_name As RoleName FROM identities.users usr JOIN identities.user_roles ur ON ur.user_id = usr.id JOIN identities.roles r ON r.id = ur.role_id LIMIT @PageSize OFFSET @Offset",
                (user, role) =>
                {
                    if (!userDict.TryGetValue(user.Email!, out var entry))
                    {
                        entry = user;
                        entry.Roles = [];
                        userDict[user.Email!] = entry;
                    }
                    entry.Roles!.Add(role);
                    return entry;
                },
                new { Offset = paginationRequest.Offset, PageSize = paginationRequest.PageSize },
                splitOn: "RoleName");
            return new PaginationDto<UserDto>
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

            var command =await connection.QueryAsync("SELECT id, email, user_name as UserName FROM identities.users WHERE id = @Id",new { Id = id });
            return command.FirstOrDefault()!;
        }


    }
}