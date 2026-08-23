using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tasky.Services.Identities.Application.Services;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Infrastructure.Configurations;
using Tasky.Services.Identities.Infrastructure.Configurations.Signing;

namespace Tasky.Services.Identities.Infrastructure.Services;


public class TokenGenerationService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ISigningKeyProvider _signingKeyProvider;
    private readonly ILogger<TokenGenerationService> _logger;

    public TokenGenerationService(
        IOptions<JwtSettings>? options,
        ISigningKeyProvider signingKeyProvider,
        ILogger<TokenGenerationService> logger)
    {
        _jwtSettings = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _signingKeyProvider = signingKeyProvider ?? 
            throw new ArgumentNullException(nameof(signingKeyProvider), 
                "Signing key provider must be registered in DI container.");
        _logger = logger;

        ValidateConfiguration();
    }

    public string GenerateToken(User user)
    {
        switch (user)
        {
            case null:
                throw new ArgumentNullException(nameof(user));
            default:
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
    }
    
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
        claims.AddRange(roles.Select(role => new Claim(System.Security.Claims.ClaimTypes.Role, role!)));

        // Add permissions (for actual authorization decisions)
        var permissions = user.Roles
            .SelectMany(r => r.Permissions)
            .Select(p => p.PermissionName)
            .Where(p => !string.IsNullOrEmpty(p))
            .Distinct();

        claims.AddRange(permissions.Select(permission => new Claim("Permission", permission!)));

        return claims;
    }

    private void ValidateConfiguration()
    {
        if (string.IsNullOrWhiteSpace(_jwtSettings.Issuer))
            throw new InvalidOperationException(
                "JWT Issuer is not configured. Set JwtSettings:Issuer in appsettings.json");

        if (_jwtSettings.Audience == null || _jwtSettings.Audience.Length == 0)
            throw new InvalidOperationException(
                "JWT Audience is not configured. Set JwtSettings:Audience in appsettings.json");

        switch (_jwtSettings.ExpiryMinutes)
        {
            case <= 0:
                throw new InvalidOperationException(
                    "JWT ExpiryMinutes must be positive. Current value: " + _jwtSettings.ExpiryMinutes);
            // More than 24 hours
            case > 1440:
                _logger.LogWarning(
                    "JWT token expiry is set to {ExpiryMinutes} minutes. " +
                    "Consider shorter expiry times for better security.",
                    _jwtSettings.ExpiryMinutes);
                break;
        }
    }

 
    private SecurityKey GetSigningKeySync()
    {
        // Note: This is a blocking call. In production with async ISigningKeyProvider implementations,
        // consider making the entire GenerateToken method async.
        return _signingKeyProvider.GetSigningKeyAsync().GetAwaiter().GetResult();
    }
}
