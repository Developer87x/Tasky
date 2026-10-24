using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Projects.Application.Commands.CreateProjectCommands;
using Tasky.Services.Projects.Application.Queries;


namespace Tasky.Services.Projects.API.Controllers;
[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectsController(
    ICommandDispatcher dispatcher,
    IProjectsQueries projectsQueries,
    ILogger<ProjectsController> logger)
    : ControllerBase
{
    [HttpPost("create-project")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(command, cancellationToken);
        return result.IsSuccess ? Created(string.Empty,result) : BadRequest(result);
    }
    [HttpGet("{projectId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]   
    
    public async Task<IActionResult> GetProject(Guid projectId, CancellationToken cancellationToken)
    {
        
        logger.LogInformation(
            "User {CurrentUserId} trying to retrieve project {TargetProjectId}",
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "unknown",
            projectId);
        
        var projects= await projectsQueries.GetProjectById(projectId);
        if(projects is null)
        {
            logger.LogError("Project with id {ProjectId} not found", projectId);
            return NotFound();
        }
        logger.LogInformation("Project with id {ProjectId} found", projectId);
        return Ok(projects);
    }
    
}