using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.CreatePermissionCommands;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableRateLimiting(RateLimitExtension.RATE_LIMIT_POLICY_FOR_AUTHENTICATED_USERS)]
public class PermissionsController(ILogger<PermissionsController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
{
    private readonly ILogger<PermissionsController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

    [HttpPost("create")]
    [Authorize(Roles = "Admins")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionCommand command)
    {
        _logger.LogInformation("Received request to create permission with name: {PermissionName}", command.PermissionName);
        var result = await _commandDispatcher.Send(command);
        _logger.LogInformation("Permission created successfully with ID: {PermissionId}", result?.Data?.PermissionId);
        return Ok(result);
    }

}