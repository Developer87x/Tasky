using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
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

}