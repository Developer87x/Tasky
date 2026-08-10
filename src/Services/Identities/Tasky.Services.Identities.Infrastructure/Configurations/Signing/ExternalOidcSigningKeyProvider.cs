using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Tasky.Services.Identities.Infrastructure.Configurations.Signing;

/// <summary>
/// Implementation of ISigningKeyProvider for external OpenID Connect identity providers.
/// Supports Azure AD, Auth0, Keycloak, IdentityServer, and other OIDC-compliant providers.
/// 
/// This implementation:
/// - Discovers signing keys from the provider's JWKS endpoint
/// - Automatically refreshes keys periodically
/// - Supports seamless key rotation by the external provider
/// - Enables zero-trust architecture with external identity providers
/// </summary>
public class ExternalOidcSigningKeyProvider : ISigningKeyProvider
{
    private readonly string _authority;
    private readonly IConfigurationManager<OpenIdConnectConfiguration> _configurationManager;
    private readonly HttpClient _httpClient;
    private OpenIdConnectConfiguration? _configuration;
    private DateTime _lastConfigurationRefresh = DateTime.MinValue;
    private readonly TimeSpan _configurationRefreshInterval = TimeSpan.FromHours(1);

    public string SigningAlgorithm => SecurityAlgorithms.RsaSha256;

    public bool IsAsymmetric => true;

    /// <summary>
    /// Initialize with an external OIDC authority.
    /// </summary>
    /// <param name="authority">The OIDC authority URL (e.g., https://login.microsoftonline.com/tenant-id/v2.0)</param>
    /// <param name="httpClient">Optional HttpClient for custom configuration</param>
    public ExternalOidcSigningKeyProvider(string authority, HttpClient? httpClient = null)
    {
        if (string.IsNullOrWhiteSpace(authority))
            throw new ArgumentNullException(nameof(authority), "OIDC authority cannot be null or empty.");

        _authority = authority.TrimEnd('/');
        _httpClient = httpClient ?? new HttpClient();

        _configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            metadataAddress: $"{_authority}/.well-known/openid-configuration",
            configRetriever: new OpenIdConnectConfigurationRetriever(),
            httpClient: _httpClient);
            
            
    }

    /// <summary>
    /// Get the signing key from the external OIDC provider.
    /// Returns the first signing key available.
    /// </summary>
    public async Task<SecurityKey> GetSigningKeyAsync()
    {
        var configuration = await GetConfigurationAsync();
        var signingKeys = configuration.SigningKeys;

        if (!signingKeys.Any())
            throw new InvalidOperationException(
                $"No signing keys found from OIDC authority: {_authority}");

        return signingKeys.First();
    }

    /// <summary>
    /// Get all valid signing keys from the external provider (supports key rotation).
    /// </summary>
    public async Task<IEnumerable<SecurityKey>> GetValidationKeysAsync()
    {
        var configuration = await GetConfigurationAsync();
        return configuration.SigningKeys;
    }

    /// <summary>
    /// Returns null for external OIDC (signing is done by the external provider).
    /// </summary>
    public Task<SigningCredentials?> GetSigningCredentialsAsync() =>
        Task.FromResult<SigningCredentials?>(null);

    /// <summary>
    /// Get or refresh OIDC configuration from the authority.
    /// Configuration is cached and refreshed periodically.
    /// </summary>
    private async Task<OpenIdConnectConfiguration> GetConfigurationAsync()
    {
        if (_configuration != null && 
            DateTime.UtcNow - _lastConfigurationRefresh < _configurationRefreshInterval)
        {
            return _configuration;
        }

        try
        {
            _configuration = await _configurationManager.GetConfigurationAsync(CancellationToken.None);
            _lastConfigurationRefresh = DateTime.UtcNow;
            return _configuration;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to retrieve OpenID Connect configuration from {_authority}. " +
                "Ensure the authority URL is correct and the OIDC endpoint is accessible.",
                ex);
        }
    }

    /// <summary>
    /// Force refresh of OIDC configuration (useful for manual key rotation handling).
    /// </summary>
    public async Task RefreshConfigurationAsync()
    {
        _configuration = null;
        _lastConfigurationRefresh = DateTime.MinValue;
        await GetConfigurationAsync();
    }
}
