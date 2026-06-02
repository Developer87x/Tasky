using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.ChangePasswordCommands;

public class ChangePasswordCommand:ICommand<ResultCommand<ChangePasswordResult>>
{
    public Guid UserId { get; set; }
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}
