using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.AssignPermissionsToRoleCommand;
using Tasky.Services.Identities.Application.Commands.CreateRoleCommands;
using Tasky.Services.Identities.Application.Queries;
using Tasky.Services.Identities.Application.Security;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableRateLimiting(RateLimitExtension.RateLimitPolicyForAuthenticatedUsers)]
[Authorize]  // Require authentication for all actions
public class RolesController(ILogger<RolesController> logger, ICommandDispatcher dispatcher, IRoleQueries roleQueries) : ControllerBase
{
    private readonly ILogger<RolesController> _logger = logger;
    private readonly ICommandDispatcher _dispatcher = dispatcher;
    private readonly IRoleQueries _roleQueries = roleQueries;

    /// <summary>
    /// Create a new role (admin-only operation).
    /// Requires: Roles.Create permission
    /// </summary>
    [HttpPost("create-role")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "User {UserId} attempting to create role {RoleName}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            command.RoleName);

        var result = await _dispatcher.DispatchAsync(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            _logger.LogWarning("Role {RoleName} created by {UserId}", command.RoleName,
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown");
            return Ok(result);
        }

        _logger.LogWarning("Failed to create role {RoleName}. Error: {Error}", command.RoleName, result.Error);
        return BadRequest(result);
    }

    /// <summary>
    /// Get all roles with pagination.
    /// Requires: Roles.Read permission
    /// </summary>
    [HttpGet("get-roles")]
    [Authorize(Policy = Permissions.Roles.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "User {UserId} retrieving roles (page {PageNumber}, size {PageSize})",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            pageNumber,
            pageSize);

        var result = await _roleQueries.GetRolesAsync(pageNumber, pageSize, cancellationToken);
        
        if (result != null)
        {
            _logger.LogInformation("Roles retrieved successfully");
            return Ok(result);
        }

        _logger.LogWarning("Failed to retrieve roles");
        return BadRequest();
    }

    /// <summary>
    /// Get a specific role by ID.
    /// Requires: Roles.Read permission
    /// </summary>
    [HttpGet("{roleId}")]
    [Authorize(Policy = Permissions.Roles.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoleById([FromRoute] Guid roleId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "User {UserId} retrieving role {RoleId}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            roleId);

        var result = await _roleQueries.GetRoleByIdAsync(roleId, cancellationToken);
        
        if (result != null)
        {
            _logger.LogInformation("Role {RoleId} retrieved successfully", roleId);
            return Ok(result);
        }

        _logger.LogWarning("Role {RoleId} not found", roleId);
        return NotFound();
    }

    [HttpPut("assign-permissions-to-role")]
    [Authorize(Policy = Permissions.Roles.AssignPermissions)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignPermissionsToRoleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogWarning(
            "User {UserId} attempting to assign {PermissionCount} permissions to role {RoleId}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            command.PermissionIds?.Count ?? 0,
            command.RoleId);

        var result = await _dispatcher.DispatchAsync(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            _logger.LogWarning(
                "Permissions assigned to role {RoleId} by {UserId}",
                command.RoleId,
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown");
            return Ok(result);
        }

        _logger.LogWarning(
            "Failed to assign permissions to role {RoleId}. Error: {Error}",
            command.RoleId,
            result.Error);
        return BadRequest(result);
    }
} 