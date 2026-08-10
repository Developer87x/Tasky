using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Tasky.Services.Identities.Infrastructure.Configurations.Signing;

/// <summary>
/// Implementation of ISigningKeyProvider using symmetric HMAC signing.
/// Suitable for development environments and internal microservices.
/// 
/// ⚠️ Security Note:
/// - The shared secret MUST be stored securely (e.g., Azure Key Vault, environment variables).
/// - Never commit secrets to source control.
/// - For production, consider migrating to asymmetric signing (RSA) or external IdP.
/// </summary>
public class SymmetricSigningKeyProvider : ISigningKeyProvider
{
    private readonly SymmetricSecurityKey _key;

    public string SigningAlgorithm => SecurityAlgorithms.HmacSha256;

    public bool IsAsymmetric => false;

    /// <summary>
    /// Initialize with a base64-encoded shared secret.
    /// </summary>
    /// <param name="secret">Base64-encoded secret or UTF-8 string (must be at least 32 bytes)</param>
    /// <exception cref="ArgumentNullException">Thrown when secret is null or empty</exception>
    /// <exception cref="ArgumentException">Thrown when secret is too short</exception>
    public SymmetricSigningKeyProvider(string secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
            throw new ArgumentNullException(nameof(secret), "Signing secret cannot be null or empty.");

        var keyBytes = Encoding.UTF8.GetBytes(secret);
        if (keyBytes.Length < 32)
            throw new ArgumentException(
                "Signing secret must be at least 32 bytes (256 bits) for HMAC-SHA256. " +
                $"Current length: {keyBytes.Length} bytes.",
                nameof(secret));

        _key = new SymmetricSecurityKey(keyBytes);
    }

    /// <summary>
    /// Get the symmetric key used for both signing and validation.
    /// </summary>
    public Task<SecurityKey> GetSigningKeyAsync() => Task.FromResult<SecurityKey>(_key);

    /// <summary>
    /// Symmetric keys are used for both signing and validation.
    /// Returns only the current key (no rotation in symmetric mode).
    /// </summary>
    public Task<IEnumerable<SecurityKey>> GetValidationKeysAsync() =>
        Task.FromResult(Enumerable.Repeat<SecurityKey>(_key, 1));

    /// <summary>
    /// Returns null to use default signing behavior with the SymmetricSecurityKey.
    /// </summary>
    public Task<SigningCredentials?> GetSigningCredentialsAsync() =>
        Task.FromResult<SigningCredentials?>(null);
}
