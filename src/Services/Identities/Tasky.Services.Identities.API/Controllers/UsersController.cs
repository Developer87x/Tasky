using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Identities.Application.Commands.ActivateUserCommands;
using Tasky.Services.Identities.Application.Commands.AssignRoleToUserCommands;
using Tasky.Services.Identities.Application.Commands.CreateUserCommands;
using Tasky.Services.Identities.Application.Queries;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableRateLimiting(RateLimitExtension.RateLimitPolicyForAuthenticatedUsers)]
[Authorize]  // Require authentication for all actions in this controller (unless overridden)
public class UsersController(ILogger<UsersController> logger, ICommandDispatcher commandDispatcher, IUserQueries userQueries) : ControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;
    private readonly IUserQueries _userQueries = userQueries;
    
    [HttpPost("create-user")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "User {UserId} attempting to create new user with username {Username}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            command.UserName);

        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation(
                "User successfully created with ID {UserId}",
                result.Value.Id);
            return CreatedAtAction(nameof(GetUserById), new { userId = result.Value.Id }, result);
        }

        _logger.LogWarning(
            "Failed to create user {Username}. Error: {Error}",
            command.UserName,
            result.Error);
        return BadRequest(result);
    }

    [HttpGet("{userId}")]
    [Authorize(Roles = Permissions.Roles.Users)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "User {CurrentUserId} retrieving user {TargetUserId}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            userId);

        var result = await _userQueries.GetUserByIdAsync(userId, cancellationToken);
        
        if (result != null)
        {
            _logger.LogInformation("User {UserId} retrieved successfully", userId);
            return Ok(result);
        }

        _logger.LogWarning("User {UserId} not found", userId);
        return NotFound();
    }
    
    [HttpPut("assign-role-to-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "User {CurrentUserId} attempting to assign role {RoleId} to user {TargetUserId}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            command.RoleId,
            command.UserId);

        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            _logger.LogWarning(  // Log as warning because this is a privilege escalation operation
                "Role {RoleId} assigned to user {UserId} by {AdminUserId}",
                command.RoleId,
                command.UserId,
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown");
            return Ok(result);
        }

        _logger.LogWarning(
            "Failed to assign role {RoleId} to user {TargetUserId}. Error: {Error}",
            command.RoleId,
            command.UserId,
            result.Error);
        return BadRequest(result);
    }
    [HttpPut("activate-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActivateUser([FromBody] ActivateUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "User {CurrentUserId} attempting to activate user {TargetUserId}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            command.UserId);

        var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation(
                "User {UserId} activated by {AdminUserId}",
                command.UserId,
                User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown");
            return Ok(result);
        }

        _logger.LogWarning(
            "Failed to activate user {TargetUserId}. Error: {Error}",
            command.UserId,
            result.Error);
        return BadRequest(result);
    }
}