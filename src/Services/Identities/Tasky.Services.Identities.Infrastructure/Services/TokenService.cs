
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tasky.Services.Identities.Application.Services;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Infrastructure.Configurations;


namespace Tasky.Services.Identities.Infrastructure.Services;

public class TokenService(IOptions<JwtSettings> options) : ITokenService
{
    private readonly JwtSettings _jwtSettings = options.Value;
    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.UserName!),
            new (ClaimTypes.Email, user.Email!.Value!),
        };
        var roles = user.Roles.Select(r => r.RoleName).Distinct();
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role!));
        }
        var permissions = user.Roles.SelectMany(r => r.Permissions).Select(p => p.PermissionName).Distinct();
        foreach (var permission in permissions)
            claims.Add(new Claim("Permission", permission!));

        var audiences = _jwtSettings.Audience ?? throw new InvalidOperationException("JWT audiences are missing.");
        if (audiences.Length == 0)
            throw new InvalidOperationException("JWT audiences are missing.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!));
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: audiences.First(),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        token.Payload[JwtRegisteredClaimNames.Aud] = audiences;
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
