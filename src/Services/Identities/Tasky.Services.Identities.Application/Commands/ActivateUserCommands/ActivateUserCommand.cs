using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.BuildingBlocks.Constants;

namespace Tasky.Services.Identities.Application.Commands.ActivateUserCommands;

public class ActivateUserCommand :ICommand<Result>
{
    public Guid UserId { get; set; }
}
