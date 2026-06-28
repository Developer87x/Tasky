using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tasky.Services.Identities.Application.Commands;
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

} 