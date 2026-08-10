using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Identities.Application.Commands.RefreshTokenCommands;
using Tasky.Services.Identities.Application.Commands.SignInCommands;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableRateLimiting(RateLimitExtension.RateLimitPolicyForAuthenticatedUsers)]
public class AuthenticateController(ILogger<AuthenticateController> logger, ICommandDispatcher dispatcher) : ControllerBase
{
    private readonly ILogger<AuthenticateController> _logger = logger;
    private readonly ICommandDispatcher _dispatcher = dispatcher;

    
    [HttpPost("sign-in")]
    [AllowAnonymous]  // Explicitly allow unauthenticated access (no token required for login)
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sign-in attempt for Email: {Email}", command.Email);

        var result = await _dispatcher.DispatchAsync(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Sign-in successful for Email: {Email}", command.Email);
            return Ok(result);
        }

        // Log failed attempts but don't leak information about whether user exists
        _logger.LogWarning(
            "Sign-in failed for Email: {Email}. Error: {Error}",
            command.Email,
            result.Error);
        return BadRequest(result);
    }

    /// <summary>
    /// Refresh an expir JWT token using a refresh token.
    /// Requires: Valid JWT token (authenticated user)
    /// 
    /// SECURITY:
    /// - Only authenticated users can refresh their tokens
    /// - Validates refresh token hasn't expired
    /// - Issues new access token with same claims
    /// - Optionally rotates refresh token for additional security
    /// </summary>
    [HttpPost("refresh-token")]
    [Authorize]  // Require valid JWT token for refresh (implicit authentication)
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown";
        _logger.LogInformation("Token refresh attempt for user: {UserId}", userId);

        var result = await _dispatcher.DispatchAsync(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Token refreshed successfully for user: {UserId}", userId);
            return Ok(result);
        }

        _logger.LogWarning("Token refresh failed for user: {UserId}. Error: {Error}", userId, result.Error);
        return BadRequest(result);
    }
}