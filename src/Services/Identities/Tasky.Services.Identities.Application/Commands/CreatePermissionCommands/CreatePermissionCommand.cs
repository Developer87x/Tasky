using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.CreatePermissionCommands;

public class CreatePermissionCommand :ICommand<ResultCommand<CreatePermissionResult>>
{
    public string? PermissionName { get; set; } = string.Empty;
}
