using Microsoft.IdentityModel.Tokens;

namespace Tasky.Services.Identities.Infrastructure.Configurations.Signing;

/// <summary>
/// Abstraction for JWT signing key resolution.
/// Supports multiple signing strategies:
/// - Symmetric (HMAC with shared secret) - Development
/// - RSA certificates - Production
/// - External Identity Providers (OIDC discovery) - Enterprise
/// 
/// This design enables zero-downtime migration from symmetric to asymmetric signing
/// and seamless integration with external identity providers.
/// </summary>
public interface ISigningKeyProvider
{
    /// <summary>
    /// Get the current signing key for creating new tokens.
    /// </summary>
    /// <returns>SecurityKey used to sign new tokens</returns>
    Task<SecurityKey> GetSigningKeyAsync();

    /// <summary>
    /// Get all valid keys for token validation (supports key rotation).
    /// Includes current and recent previous keys to handle key rotation gracefully.
    /// </summary>
    /// <returns>Collection of SecurityKey objects valid for token validation</returns>
    Task<IEnumerable<SecurityKey>> GetValidationKeysAsync();

    /// <summary>
    /// Get the signing algorithm (e.g., "RS256", "HS256")
    /// </summary>
    string SigningAlgorithm { get; }

    /// <summary>
    /// Indicates whether this provider uses asymmetric signing (RSA, ECDSA).
    /// Used to determine if public keys should be exposed via JWKS endpoint.
    /// </summary>
    bool IsAsymmetric { get; }

    /// <summary>
    /// Optional: Get the signing credentials for token creation.
    /// Used when custom SigningCredentials are needed beyond basic SecurityKey.
    /// </summary>
    /// <returns>SigningCredentials if available; null for default behavior</returns>
    Task<SigningCredentials?> GetSigningCredentialsAsync();
}
