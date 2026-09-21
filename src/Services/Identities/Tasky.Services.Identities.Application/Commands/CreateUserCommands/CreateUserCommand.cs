using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.CreateUserCommands;

public class CreateUserCommand : ICommand<Result<UserDto>>
{
    public string? Email { get; set; } 
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; } 
    public string? Password { get; set; } 
}
