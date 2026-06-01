using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.LoginUserCommands;
using Tasky.Services.Identities.Application.Commands.RefreshTokenCommands;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableRateLimiting(RateLimitExtension.RATE_LIMIT_POLICY_FOR_AUTHENTICATED_USERS)]
public class AuthenticateController(ILogger<AuthenticateController> logger, ICommandDispatcher dispatcher) : ControllerBase
{
    private readonly ILogger<AuthenticateController> _logger = logger;
    private readonly ICommandDispatcher _dispatcher = dispatcher;

    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        _logger.LogInformation("Received login request for user: {userName}", command.UserName);
        var result = await _dispatcher.Send(command);
        _logger.LogInformation("Login result for user {userName}: {result}", command.UserName, result);
        return Ok(result);
    }

    [HttpPost("RefreshToken")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await _dispatcher.Send(command);
        return Ok(result);
    }   
}