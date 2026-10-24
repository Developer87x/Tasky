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
        _logger.LogInformation("Creating category with name {@Command}", command);
        var result = await dispatcher.DispatchAsync(command, cancellationToken);
        if (!result.IsSuccess)
        {
            _logger.LogError("Category creation failed with error {Error}", result.Error);
            return BadRequest(result);
        }
        _logger.LogInformation("Category created with id {@Result}", result);
        return Created(string.Empty, result);
    }
}