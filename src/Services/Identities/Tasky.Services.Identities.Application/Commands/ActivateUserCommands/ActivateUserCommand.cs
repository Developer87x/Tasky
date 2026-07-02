using Tasky.Services.Identities.Application.Common;

namespace Tasky.Services.Identities.Application.Commands.ActivateUserCommands;

public class ActivateUserCommand :ICommand<Result>
{
    public Guid UserId { get; set; }
}
