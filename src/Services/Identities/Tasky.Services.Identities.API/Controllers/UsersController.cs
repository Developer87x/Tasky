using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.ChangePasswordCommands;
using Tasky.Services.Identities.Application.Commands.CreateUserCommands;
using Tasky.Services.Identities.Application.Commands.UpdateUserCommands;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Application.Queries;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;
namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableRateLimiting(RateLimitExtension.RATE_LIMIT_POLICY_FOR_AUTHENTICATED_USERS)]

public class UsersController(ILogger<UsersController> logger, ICommandDispatcher commandDispatcher, IUserQueries userQueries) : ControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;
    private readonly IUserQueries _userQueries = userQueries;

    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        _logger.LogInformation("Received request to create a new user with email: {user}", command);
        var result = await _commandDispatcher.Send(command);
        _logger.LogInformation("User creation result: {result}", result);
        return Ok(result);
    }
    [HttpGet("users-list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUsersList([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation("Received request to get users list with pagination: page={page}, pageSize={pageSize}", page, pageSize);
        var pagination = new PaginationRequest { Page = page, PageSize = pageSize };
        var list = await _userQueries.GetAllUserAsync(pagination);
        _logger.LogInformation("Retrieved {count} users from the database", list.TotalCount);
        return Ok(list);
    }
    [HttpGet("user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        _logger.LogInformation("Received request to get user by id: {id}", id);
        var user = await _userQueries.GetUserById(id);
        if (user == null)
        {
            _logger.LogWarning("User with id {id} not found", id);
            return NotFound();
        }
        _logger.LogInformation("User with id {id} retrieved successfully", id);
        return Ok(user);
    }
    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
    {
        _logger.LogInformation("Received request to update user with id: {id}", command.UserId);
        var result = await _commandDispatcher.Send(command);
        _logger.LogInformation("User update result for id {id}: {result}", command.UserId, result);
        return Ok(result);
    }
    [HttpPut("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        _logger.LogInformation("Received request to change password for user with id: {id}", command.UserId);
        var result = await _commandDispatcher.Send(command);
        _logger.LogInformation("Password change result for user id {id}: {result}", command.UserId, result);
        return Ok(result);
    }

}