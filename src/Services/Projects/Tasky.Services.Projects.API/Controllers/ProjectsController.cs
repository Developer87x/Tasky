using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Projects.Application.Commands.CreateProjectCommands;

namespace Tasky.Services.Projects.API.Controllers;
[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectsController :ControllerBase
{
    private readonly ICommandDispatcher _dispatcher;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(ICommandDispatcher dispatcher,ILogger<ProjectsController> logger)
    {
        _dispatcher =dispatcher;
        _logger =logger;
    }

    [HttpPost("create-project")]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand comand, CancellationToken cancellationToken)
    {
        var result = await _dispatcher.DispatchAsync(comand, cancellationToken);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}