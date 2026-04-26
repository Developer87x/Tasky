using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.LoginUserCommands;
using Tasky.Services.Identities.Application.Commands.RefreshTokenCommands;
using Tasky.Services.Identities.Infrastructure.Configurations.ServicesExtensions;

namespace Tasky.Services.Identities.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthenticateController(ICommandDispatcher dispatcher) : ControllerBase
    {
        [HttpPost("login")]
        [EnableRateLimiting(RateLimitExtension.RATE_LIMIT_POLICY_FOR_AUTHENTICATED_USERS)]
        public async Task<IActionResult> Login([FromBody]LoginUserCommand command)
        {
            var result = await dispatcher.Send(command);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
    [EnableRateLimiting(RateLimitExtension.RATE_LIMIT_POLICY_FOR_AUTHENTICATED_USERS)]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenCommand command)
        {
            var result = await dispatcher.Send(command);
            return Ok(result);
        }
    }
}