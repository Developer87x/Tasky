using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
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

}