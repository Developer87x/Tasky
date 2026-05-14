using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasky.Services.Identities.Application.Commands;
using Tasky.Services.Identities.Application.Commands.CreateUserCommands;
using Tasky.Services.Identities.Application.Commands.UpdateUserCommands;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Application.Queries;
namespace Tasky.Services.Identities.API.Controllers;
[ApiController]
[Route("api/[controller]")]

public class UsersController : ControllerBase
{
}