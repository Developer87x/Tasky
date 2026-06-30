using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.CreateUserCommands;
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


    [HttpPost("create-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("the process of creating a new user has started"); // Log the start of the user creation process
        var result = await _commandDispatcher.Send(command, cancellationToken); // Send the command to the command dispatcher for processing
        if (result.IsSuccess) // Check if the result indicates success
        {
            _logger.LogInformation("the process of creating a new user has completed successfully"); // Log the successful completion of the user creation process
            return Ok(result); // Return the result if successful
        }
        else
        {
            _logger.LogError("the process of creating a new user has failed with error: {Error}", result.Error); // Log the error if the user creation process failed
            return BadRequest(result); // Return the error if failed
        }
    }

    // GET: api/users/{userId}
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("the process of retrieving user with ID {UserId} has started", userId); // Log the start of the user retrieval process
        var result = await _userQueries.GetUserByIdAsync(userId, cancellationToken); // Retrieve the user by ID using the user queries service
        if (result != null) // Check if the result is not null
        {
            _logger.LogInformation("the process of retrieving user with ID {UserId} has completed successfully", userId); // Log the successful completion of the user retrieval process
            return Ok(result); // Return the result if found
        }
        else
        {
            _logger.LogWarning("user with ID {UserId} was not found", userId); // Log a warning if the user was not found
            return NotFound(); // Return a 404 Not Found response if the user was not found
        }
    }

}