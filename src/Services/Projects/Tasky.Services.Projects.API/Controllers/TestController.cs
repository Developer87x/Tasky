using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Projects.Application.Commands.CreateCategoryCommands;

namespace Tasky.Services.Projects.API.Controllers;

[ApiController]
[Route("api/[controller]")]
    
public class TestController :ControllerBase
{
    [Authorize(Policy=Permissions.Permission.FullAccess)] 
        
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("test-api")]
    public IActionResult Get()
    {
        return Ok("Test endpoint is working!");
    }
}

[ApiController]
[Route("api/[controller]")]
public class CategoryController :ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ICommandDispatcher _dispatcher;


    public CategoryController(ILogger<CategoryController> logger, ICommandDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }
    
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("Create-category")]
    public async Task<IActionResult> Post([FromBody] CreateCategoryCommand command,CancellationToken cancellationToken)
    {
        var result = await _dispatcher.DispatchAsync(command, cancellationToken);
        if(result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}