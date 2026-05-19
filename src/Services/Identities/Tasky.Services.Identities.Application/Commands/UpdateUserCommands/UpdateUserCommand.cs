using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.UpdateUserCommands;

public class UpdateUserCommand :ICommand<ResultCommand<UpdateUserCommandResult>>
{
    public string UserId { get; set; } = string.Empty;
    public string? Email { get; set; }
}
