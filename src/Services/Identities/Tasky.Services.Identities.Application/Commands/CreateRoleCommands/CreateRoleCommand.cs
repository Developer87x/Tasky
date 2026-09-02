using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;

namespace Tasky.Services.Identities.Application.Commands.CreateRoleCommands;

public class CreateRoleCommand :ICommand<Result>
{
    public string? RoleName { get; set; }
}