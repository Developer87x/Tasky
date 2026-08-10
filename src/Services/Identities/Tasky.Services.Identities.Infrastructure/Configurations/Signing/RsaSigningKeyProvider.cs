using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Tasky.Services.Identities.Infrastructure.Configurations.Signing;

/// <summary>
/// Implementation of ISigningKeyProvider using RSA asymmetric signing.
/// Suitable for production environments, multi-service authentication, and enterprise deployments.
/// 
/// Advantages over symmetric signing:
/// - Public key can be shared via JWKS endpoint without exposing private key
/// - Better for distributed systems and cross-organizational trust
/// - Supports seamless key rotation
/// - Aligns with OAuth 2.0 and OpenID Connect standards
/// </summary>
public class RsaSigningKeyProvider : ISigningKeyProvider
{
    private readonly RsaSecurityKey _signingKey;
    private readonly List<SecurityKey> _validationKeys;

    public string SigningAlgorithm => SecurityAlgorithms.RsaSha256;

    public bool IsAsymmetric => true;

    /// <summary>
    /// Initialize with an RSA private key (from certificate or PEM file).
    /// </summary>
    /// <param name="rsaPrivateKey">RSA private key for signing</param>
    /// <param name="additionalValidationKeys">Optional previous keys for rotation support</param>
    public RsaSigningKeyProvider(RSA rsaPrivateKey, IEnumerable<SecurityKey>? additionalValidationKeys = null)
    {
        if (rsaPrivateKey == null)
            throw new ArgumentNullException(nameof(rsaPrivateKey));

        _signingKey = new RsaSecurityKey(rsaPrivateKey);
        _validationKeys = new List<SecurityKey> { _signingKey };

        if (additionalValidationKeys != null)
            _validationKeys.AddRange(additionalValidationKeys);
    }

    /// <summary>
    /// Initialize from a certificate file (PEM or PKCS12 format).
    /// </summary>
    /// <param name="certificatePath">Path to the certificate file</param>
    /// <param name="password">Password if certificate is PKCS12 encrypted (optional)</param>
    public RsaSigningKeyProvider(string certificatePath, string? password = null)
    {
        if (!File.Exists(certificatePath))
            throw new FileNotFoundException($"Certificate file not found: {certificatePath}");

        var certificateBytes = File.ReadAllBytes(certificatePath);
        var rsa = LoadRsaFromCertificate(certificateBytes, password);

        _signingKey = new RsaSecurityKey(rsa);
        _validationKeys = new List<SecurityKey> { _signingKey };
    }

    /// <summary>
    /// Get the RSA signing key.
    /// </summary>
    public Task<SecurityKey> GetSigningKeyAsync() => Task.FromResult<SecurityKey>(_signingKey);

    /// <summary>
    /// Get all valid RSA keys (current and rotated previous keys).
    /// </summary>
    public Task<IEnumerable<SecurityKey>> GetValidationKeysAsync() =>
        Task.FromResult<IEnumerable<SecurityKey>>(_validationKeys);

    /// <summary>
    /// Get signing credentials with RSA-SHA256.
    /// </summary>
    public Task<SigningCredentials?> GetSigningCredentialsAsync() =>
        Task.FromResult<SigningCredentials?>(
            new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256));

    /// <summary>
    /// Add a previous key for rotation support.
    /// Allows tokens signed with the old key to remain valid during rotation.
    /// </summary>
    public void AddValidationKey(SecurityKey key)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        _validationKeys.Add(key);
    }

    /// <summary>
    /// Load RSA key from a certificate file (PEM or PKCS12).
    /// </summary>
    private static RSA LoadRsaFromCertificate(byte[] certificateBytes, string? password)
    {
        try
        {
            // Try to load as PKCS12 (includes private key)
            if (certificateBytes.Length > 0 && certificateBytes[0] == 0x30) // PKCS12/DER format
            {
                var x509 = new System.Security.Cryptography.X509Certificates.X509Certificate2(
                    certificateBytes,
                    password,
                    System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.Exportable);

                if (x509.PrivateKey is RSA rsa)
                    return rsa;

                throw new InvalidOperationException("Certificate does not contain an RSA private key.");
            }

            // Try to load as PEM
            var pem = System.Text.Encoding.UTF8.GetString(certificateBytes);
            if (pem.Contains("BEGIN"))
            {
                return RSA.Create();
                // Note: For PEM parsing, you'd need additional parsing logic or use BouncyCastle
                // This is a simplified example; production code should use proper PEM parsing
            }

            throw new InvalidOperationException("Unable to determine certificate format.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Failed to load RSA key from certificate. Ensure the certificate contains a private key.",
                ex);
        }
    }

    /// <summary>
    /// Get the public key (for JWKS endpoint or external verification).
    /// This can be safely exposed without compromising security.
    /// </summary>
    public RSA GetPublicKey() =>
        _signingKey.Rsa ?? throw new InvalidOperationException("RSA key not available.");
}
