using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.CreateRoleCommands;

public class CreateRoleCommand :ICommand<ResultCommand<CreateRoleResult>>
{
    public string? RoleName { get;  set; }
}
