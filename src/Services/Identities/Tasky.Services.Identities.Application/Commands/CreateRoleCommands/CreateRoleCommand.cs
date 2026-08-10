using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Identities.Application.Common;

namespace Tasky.Services.Identities.Application.Commands.CreateRoleCommands;

public class CreateRoleCommand :ICommand<Result>
{
    public string? RoleName { get; set; }
}