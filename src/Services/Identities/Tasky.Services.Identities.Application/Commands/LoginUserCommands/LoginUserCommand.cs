using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.LoginUserCommands;

public class LoginUserCommand :ICommand<ResultCommand<LoginCommandResult>>
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}
