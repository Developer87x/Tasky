using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Queries;

public interface IUserQueries
{
    Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
}