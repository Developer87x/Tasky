using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.CreateRoleCommands;
using Tasky.Services.Identities.Application.Queries;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
[EnableRateLimiting(RateLimitExtension.RATE_LIMIT_POLICY_FOR_AUTHENTICATED_USERS)]
public class RolesController(ILogger<RolesController> logger, ICommandDispatcher dispatcher, IRoleQueries roleQueries) : ControllerBase
{
    private readonly ILogger<RolesController> _logger = logger;
    private readonly ICommandDispatcher _dispatcher = dispatcher;
    private readonly IRoleQueries _roleQueries = roleQueries;

    [HttpPost("create-role")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("the process of creating a new role has started"); // Log the start of the role creation process
        var result = await _dispatcher.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            _logger.LogInformation("the process of creating a new role has completed successfully"); // Log the successful completion of the role creation process
            return Ok(result);
        }
        _logger.LogWarning("the process of creating a new role has failed"); // Log the failure of the role creation process
        return BadRequest(result);
    }
} 