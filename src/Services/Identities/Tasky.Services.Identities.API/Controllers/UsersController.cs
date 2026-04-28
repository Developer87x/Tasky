using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.CreateUserCommands;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Application.Queries;

namespace Tasky.Services.Identities.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(ICommandDispatcher dispatcher,IUserQueries userQueries) : ControllerBase
{

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IActionResult> Post([FromBody] CreateUserCommand command)
    {
        var result = await dispatcher.Send(command);
        return Ok(result);
    }
    [HttpPost("users-list")]
    public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var pagination = new PaginationRequestDto { Page = page, PageSize = pageSize };
        var list =await userQueries.GetAllUserAsync(pagination);
        return Ok(list);
    }


}