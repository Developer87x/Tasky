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
public class RolesController : ControllerBase
{

}