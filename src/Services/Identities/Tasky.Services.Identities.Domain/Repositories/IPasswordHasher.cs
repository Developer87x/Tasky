namespace Tasky.Services.Identities.Domain.Repositories;

public interface IPasswordHasher
{
    Task<string> HashPasswordAsync(string password);
    Task<bool> VerifyPasswordAsync(string hashedPassword, string providedPassword, CancellationToken cancellationToken = default);
}