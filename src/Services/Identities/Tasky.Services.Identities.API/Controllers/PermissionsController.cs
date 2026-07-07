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

    [HttpPost("create-permission")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionCommand command)
    {
        _logger.LogInformation("Received request to create permission with name: {PermissionName}", command.Name);
        var result =await _commandDispatcher.Send(command);
        if(result.IsSuccess)
        {
            _logger.LogInformation("Permission created successfully with name: {PermissionName}", command.Name);
            return CreatedAtAction(nameof(CreatePermission), new { name = command.Name }, result);
        }
        _logger.LogWarning("Failed to create permission with name: {PermissionName}. Error: {Error}", command.Name, result.Error);
        return BadRequest(result);
    }
}