using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public interface IRoleQueries
{
    Task<PaginationDto<RoleDto>> GetAllRolesAsync(PaginationRequestDto paginationRequest);
    
}
