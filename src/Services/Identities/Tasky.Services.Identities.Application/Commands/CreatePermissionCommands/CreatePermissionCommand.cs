using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.CreatePermissionCommands;

public class CreatePermissionCommand :ICommand<ResultDto<PermissionResultDto>>
{
    public string? PermissionName { get; set; }
}
