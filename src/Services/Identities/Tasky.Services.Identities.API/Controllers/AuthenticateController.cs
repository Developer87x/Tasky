using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
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

}