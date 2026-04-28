using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.CreateRoleCommands;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Application.Queries;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Authorize(Roles ="Admins")]
[Route("api/[controller]")] 
public class RolesController(IRoleQueries roleQueries,ICommandDispatcher commandDispatcher) : ControllerBase
{

    [HttpGet("roles-list")]
    public async Task<IActionResult> GetRoles([FromQuery] int page =1, [FromQuery] int pageSize = 10)
    {
        var paginationRequest = new PaginationRequestDto { Page = page, PageSize = pageSize };
        var roles = await roleQueries.GetAllRolesAsync(paginationRequest);
        return Ok(roles);
    }
    [HttpPost("create")]
    public async Task<IActionResult> Post([FromBody] CreateRoleCommand command)
    {
        var result = await commandDispatcher.Send(command);
        return Ok(result);
    }
}