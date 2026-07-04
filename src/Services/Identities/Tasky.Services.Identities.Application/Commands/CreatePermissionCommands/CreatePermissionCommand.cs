using Tasky.Services.Identities.Application.Common;

namespace Tasky.Services.Identities.Application.Commands.CreatePermissionCommands;

public class CreatePermissionCommand:ICommand<Result>
{
    public string? Name { get; set; }
}
