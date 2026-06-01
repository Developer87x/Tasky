using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.AssignRoleToUserCommands;
using Tasky.Services.Identities.Application.Commands.CreateRoleCommands;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Application.Queries;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Authorize(Roles = "Admins")]
[Route("api/[controller]")]
[EnableRateLimiting(RateLimitExtension.RATE_LIMIT_POLICY_FOR_AUTHENTICATED_USERS)]
public class RolesController(ILogger<RolesController> logger, ICommandDispatcher dispatcher, IRoleQueries roleQueries) : ControllerBase
{
    private readonly ILogger<RolesController> _logger = logger;
    private readonly ICommandDispatcher _dispatcher = dispatcher;
    private readonly IRoleQueries _roleQueries = roleQueries;

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
    {
        _logger.LogInformation("Received request to create a new role with name: {role}", command);
        var result = await _dispatcher.Send(command);
        _logger.LogInformation("Role creation result: {result}", result);
        return Ok(result);
    }
    [HttpGet("roles-list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRolesList([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Received request to get roles list with pagination: page={page}, pageSize={pageSize}", page, pageSize);
        var pagination = new PaginationRequest { Page = page, PageSize = pageSize };
        var list = await _roleQueries.GetAllRolesAsync(pagination);
        _logger.LogInformation("Retrieved {count} roles from the database", list.TotalCount);
        return Ok(list);
    }
    [HttpGet("role/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleById(Guid id)
    {
        _logger.LogInformation("Received request to get role by id: {id}", id);
        var role = await _roleQueries.GetRoleById(id);
        if (role == null)
        {
            _logger.LogWarning("Role with id {id} not found", id);
            return NotFound();
        }
        _logger.LogInformation("Role with id {id} retrieved successfully", id);
        return Ok(role);
    }
    [HttpPost("assign-role-to-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserCommand command)
    {
        _logger.LogInformation("Received request to assign role to user: {command}", command);
        var result = await _dispatcher.Send(command);
        _logger.LogInformation("Assign role to user result: {result}", result);
        return Ok(result);
    }
} 