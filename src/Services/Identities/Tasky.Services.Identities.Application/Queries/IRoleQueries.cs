using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public interface IRoleQueries
{
    Task<Pagination<RoleDto>> GetAllRolesAsync(PaginationRequest paginationRequest);
    Task<RoleDto?> GetRoleById(Guid id);

}
