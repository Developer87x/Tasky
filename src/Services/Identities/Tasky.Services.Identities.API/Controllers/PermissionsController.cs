using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Identities.Application.Commands.CreatePermissionCommands;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;

/// <summary>
/// Permissions management API endpoints.
/// 
/// SECURITY: Permission management is a critical administrative operation.
/// All endpoints require explicit PermissionManagement.Create permission.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableRateLimiting(RateLimitExtension.RateLimitPolicyForAuthenticatedUsers)]
[Authorize]  // Require authentication for all actions
public class PermissionsController(ILogger<PermissionsController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
{
    private readonly ILogger<PermissionsController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;


    [HttpPost("create-permission")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionCommand command,CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "User {UserId} attempting to create permission: {PermissionName}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            command.Name);

        var result = await _commandDispatcher.DispatchAsync(command,cancellationToken);
        
        if (result.IsSuccess)
        {
            _logger.LogWarning(
                "Permission {PermissionName} created by {UserId}",
                command.Name,
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown");
            return CreatedAtAction(nameof(CreatePermission), new { name = command.Name }, result);
        }

        _logger.LogWarning(
            "Failed to create permission {PermissionName}. Error: {Error}",
            command.Name,
            result.Error);
        return BadRequest(result);
    }
}