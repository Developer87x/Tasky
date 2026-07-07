using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.RefreshTokenCommands;
using Tasky.Services.Identities.Application.Commands.SignInCommands;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[EnableRateLimiting(RateLimitExtension.RATE_LIMIT_POLICY_FOR_AUTHENTICATED_USERS)]
[AllowAnonymous] 
public class AuthenticateController(ILogger<AuthenticateController> logger, ICommandDispatcher dispatcher) : ControllerBase
{
    private readonly ILogger<AuthenticateController> _logger = logger;
    private readonly ICommandDispatcher _dispatcher = dispatcher;
    
    [HttpPost("sign-in")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("the process of signing in has started"); // Log the start of the sign-in process
        var result = await _dispatcher.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            _logger.LogInformation("the process of signing in has completed successfully"); // Log the successful completion of the sign-in process
            return Ok(result);  
        }
        _logger.LogWarning("the process of signing in has failed"); // Log the failure of the sign-in process
        return BadRequest(result);
    }
    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("the process of refreshing token has started"); // Log the start of the refresh token process
        var result = await _dispatcher.Send(command, cancellationToken);
        if (result.IsSuccess)
        {
            _logger.LogInformation("the process of refreshing token has completed successfully"); // Log the successful completion of the refresh token process
            return Ok(result);  
        }
        _logger.LogWarning("the process of refreshing token has failed"); // Log the failure of the refresh token process
        return BadRequest(result);
    }
}