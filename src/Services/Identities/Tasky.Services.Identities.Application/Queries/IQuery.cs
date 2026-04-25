using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public interface IUserQueries
{
    Task<UserDto> GetUserById(Guid id);
    Task<PaginationDto<UserDto>> GetAllUserAsync(PaginationRequestDto paginationRequest);
}