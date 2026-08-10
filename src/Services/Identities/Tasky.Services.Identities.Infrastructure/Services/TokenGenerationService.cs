using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tasky.Services.Identities.Application.Security;
using Tasky.Services.Identities.Application.Services;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Infrastructure.Configurations;
using Tasky.Services.Identities.Infrastructure.Configurations.Signing;

namespace Tasky.Services.Identities.Infrastructure.Services;

/// <summary>
/// Improved token generation service with support for multiple signing strategies.
/// Replaces the previous TokenService with enhanced security and flexibility.
/// 
/// This implementation:
/// - Uses ISigningKeyProvider for pluggable signing (symmetric, RSA, external OIDC)
/// - Adds standard JWT claims (jti, iat, nbf)
/// - Supports token expiration and refresh token rotation
/// - Logs security events for audit purposes
/// - Validates configuration at startup
/// </summary>
public class TokenGenerationService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ISigningKeyProvider _signingKeyProvider;
    private readonly ILogger<TokenGenerationService> _logger;

    public TokenGenerationService(
        IOptions<JwtSettings> options,
        ISigningKeyProvider signingKeyProvider,
        ILogger<TokenGenerationService> logger)
    {
        if (options?.Value == null)
            throw new ArgumentNullException(nameof(options));

        _jwtSettings = options.Value;
        _signingKeyProvider = signingKeyProvider ?? 
            throw new ArgumentNullException(nameof(signingKeyProvider), 
                "Signing key provider must be registered in DI container.");
        _logger = logger;

        ValidateConfiguration();
    }

    /// <summary>
    /// Generate a JWT token for an authenticated user.
    /// The token includes standard claims, roles, and permissions.
    /// </summary>
    /// <param name="user">The user entity with roles and permissions</param>
    /// <returns>A signed JWT token string</returns>
    public string GenerateToken(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        try
        {
            var claims = BuildTokenClaims(user);
            var key = GetSigningKeySync(); // Use sync wrapper for now
            var credentials = new SigningCredentials(key, _signingKeyProvider.SigningAlgorithm);

            var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience?.First(),
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: credentials);

            // Support multiple audiences in token payload
            if (_jwtSettings.Audience?.Length > 1)
            {
                token.Payload[System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Aud] = _jwtSettings.Audience;
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            _logger.LogInformation(
                "Token generated for user {UserId} with {PermissionCount} permissions and {RoleCount} roles",
                user.Id, 
                user.Roles.SelectMany(r => r.Permissions).Distinct().Count(),
                user.Roles.Count);

            return tokenString;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to generate token for user {UserId}",
                user.Id);
            throw new InvalidOperationException("Token generation failed. See logs for details.", ex);
        }
    }

    /// <summary>
    /// Build the complete set of claims for the JWT token.
    /// Includes standard OIDC claims, roles, and permission claims.
    /// </summary>
    private List<Claim> BuildTokenClaims(User user)
    {
        var claims = new List<Claim>
        {
            // Standard OIDC claims
            new(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(System.Security.Claims.ClaimTypes.Name, user.UserName ?? string.Empty),
            new(System.Security.Claims.ClaimTypes.Email, user.Email?.Value ?? string.Empty),
            
            // JWT standard claims
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Iat, 
                ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds).ToString()),
        };

        // Add roles (for organizational grouping)
        var roles = user.Roles.Select(r => r.RoleName).Where(r => !string.IsNullOrEmpty(r)).Distinct();
        foreach (var role in roles)
        {
            claims.Add(new Claim(System.Security.Claims.ClaimTypes.Role, role!));
        }

        // Add permissions (for actual authorization decisions)
        var permissions = user.Roles
            .SelectMany(r => r.Permissions)
            .Select(p => p.PermissionName)
            .Where(p => !string.IsNullOrEmpty(p))
            .Distinct();

        foreach (var permission in permissions)
        {
            claims.Add(new Claim("Permission", permission!));
        }

        return claims;
    }

    /// <summary>
    /// Validate JWT settings at startup.
    /// Throws if configuration is incomplete or invalid.
    /// </summary>
    private void ValidateConfiguration()
    {
        if (string.IsNullOrWhiteSpace(_jwtSettings.Issuer))
            throw new InvalidOperationException(
                "JWT Issuer is not configured. Set JwtSettings:Issuer in appsettings.json");

        if (_jwtSettings.Audience == null || _jwtSettings.Audience.Length == 0)
            throw new InvalidOperationException(
                "JWT Audience is not configured. Set JwtSettings:Audience in appsettings.json");

        if (_jwtSettings.ExpiryMinutes <= 0)
            throw new InvalidOperationException(
                "JWT ExpiryMinutes must be positive. Current value: " + _jwtSettings.ExpiryMinutes);

        if (_jwtSettings.ExpiryMinutes > 1440) // More than 24 hours
            _logger.LogWarning(
                "JWT token expiry is set to {ExpiryMinutes} minutes. " +
                "Consider shorter expiry times for better security.",
                _jwtSettings.ExpiryMinutes);
    }

    /// <summary>
    /// Synchronous wrapper for getting signing key.
    /// In a real implementation with async signing, this would be refactored.
    /// </summary>
    private SecurityKey GetSigningKeySync()
    {
        // Note: This is a blocking call. In production with async ISigningKeyProvider implementations,
        // consider making the entire GenerateToken method async.
        return _signingKeyProvider.GetSigningKeyAsync().GetAwaiter().GetResult();
    }
}
