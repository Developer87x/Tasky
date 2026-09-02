using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;

namespace Tasky.Services.Identities.Application.Commands.CreatePermissionCommands;

public class CreatePermissionCommand:ICommand<Result>
{
    public string? Name { get; set; }
}
