using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public interface IUserQueries
{
    Task<UserDto> GetUserById(Guid id);
    Task<Pagination<UserDto>> GetAllUserAsync(PaginationRequest paginationRequest);
}