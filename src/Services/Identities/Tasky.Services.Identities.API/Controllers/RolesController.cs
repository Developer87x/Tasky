using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.AssignPermissionsToRoleCommand;
using Tasky.Services.Identities.Application.Commands.CreateRoleCommands;
using Tasky.Services.Identities.Application.Queries;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles="Administrators")]
[Authorize(Policy = "Permission:Full")]
[EnableRateLimiting(RateLimitExtension.RATE_LIMIT_POLICY_FOR_AUTHENTICATED_USERS)]
public class RolesController(ILogger<RolesController> logger, ICommandDispatcher dispatcher, IRoleQueries roleQueries) : ControllerBase
{
    private readonly ILogger<RolesController> _logger = logger;
    private readonly ICommandDispatcher _dispatcher = dispatcher;
    private readonly IRoleQueries _roleQueries = roleQueries;

    [HttpPost("create-role")]
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
    [HttpGet("get-roles")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoles([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("the process of retrieving roles has started"); // Log the start of the role retrieval process
        var result = await _roleQueries.GetRolesAsync(pageNumber, pageSize, cancellationToken);
        if (result != null)
        {
            _logger.LogInformation("the process of retrieving roles has completed successfully"); // Log the successful completion of the role retrieval process
            return Ok(result);  
        }
        _logger.LogWarning("the process of retrieving roles has failed"); // Log the failure of the role retrieval process
        return BadRequest();    
    }
    [HttpGet("{roleId}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleById([FromRoute] Guid roleId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("the process of retrieving a role by ID has started"); // Log the start of the role retrieval process
        var result = await _roleQueries.GetRoleByIdAsync(roleId, cancellationToken);
        if (result != null)
        {
            _logger.LogInformation("the process of retrieving a role by ID has completed successfully"); // Log the successful completion of the role retrieval process
            return Ok(result);  
        }
        _logger.LogWarning("the process of retrieving a role by ID has failed"); // Log the failure of the role retrieval process
        return NotFound();    
    }
    [HttpPut("assign-permissions-to-role")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignPermissionsToRole([FromBody] AssignPermissionsToRoleCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("the process of assigning permissions to a role has started"); // Log the start of the permission assignment process
        var result = await _dispatcher.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            _logger.LogInformation("the process of assigning permissions to a role has completed successfully"); // Log the successful completion of the permission assignment process
            return Ok(result);
        }
        _logger.LogWarning("the process of assigning permissions to a role has failed"); // Log the failure of the permission assignment process
        return BadRequest(result);
    }
} 