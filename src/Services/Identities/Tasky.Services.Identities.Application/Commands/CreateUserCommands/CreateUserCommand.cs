using Tasky.Services.Identities.Application.Common;

namespace Tasky.Services.Identities.Application.Commands.CreateUserCommands;

public class CreateUserCommand : ICommand<Result>
{
    public string? Email { get; set; } = default;
    public string? UserName { get; set; } = default;
    public string? Password { get; set; } = default;
}
