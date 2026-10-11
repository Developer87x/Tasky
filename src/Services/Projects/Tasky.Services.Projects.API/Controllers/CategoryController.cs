using Microsoft.AspNetCore.Mvc;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Projects.Application.Commands.CreateCategoryCommands;

namespace Tasky.Services.Projects.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ILogger<CategoryController> logger, ICommandDispatcher dispatcher)
    : ControllerBase
{
    private readonly ILogger<CategoryController> _logger = logger;


    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("Create-category")]
    public async Task<IActionResult> Post([FromBody] CreateCategoryCommand command,CancellationToken cancellationToken)
    {
        var result = await dispatcher.DispatchAsync(command, cancellationToken);
        if(result.IsSuccess)
        {
            return Created(string.Empty, result);
        }
        return BadRequest(result);
    }
}