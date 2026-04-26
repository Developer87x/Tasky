using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}