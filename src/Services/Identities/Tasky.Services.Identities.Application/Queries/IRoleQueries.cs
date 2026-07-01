using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public interface IRoleQueries
{
    Task<PaginatedResult<RoleDto>> GetRolesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<RoleDto?> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken);
    
}
