using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Infrastructure.Persistence.Repositories;

public class PasswordHasher : IPasswordHasher
{
    private const int SALTSIZE = 16; // 128 bit
    private const int HASHSIZE = 32; // 256 bit
    private const int ITERATIONS = 3;
    private const int MEMORYSIZE = 65536; // 64 mb
    private const int PARALLELISM = 2;

    public async Task<string> HashPasswordAsync(string password)
    {
        var salt= RandomNumberGenerator.GetBytes(SALTSIZE);
        var argon = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            Iterations = ITERATIONS,
            MemorySize = MEMORYSIZE,
            DegreeOfParallelism = PARALLELISM
        };
        var hash = await argon.GetBytesAsync(HASHSIZE);
        var result = new byte[SALTSIZE + HASHSIZE];
        salt.CopyTo(result, 0);
        hash.CopyTo(result, SALTSIZE);
        return Convert.ToBase64String(result);
    }
    public async Task<bool> VerifyPasswordAsync(string hashedPassword, string providedPassword)
    {
        var decoded =Convert.FromBase64String(hashedPassword);
        var salt = decoded[..SALTSIZE];
        var hash = decoded[SALTSIZE..];
        var expectedHash = new Argon2id(Encoding.UTF8.GetBytes(providedPassword))
        {
            Salt = salt,
            Iterations = ITERATIONS,
            MemorySize = MEMORYSIZE,
            DegreeOfParallelism = PARALLELISM
        };
        var ectualHash = await expectedHash.GetBytesAsync(HASHSIZE);
        return CryptographicOperations.FixedTimeEquals(hash, ectualHash);
    }
}