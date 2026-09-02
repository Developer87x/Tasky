using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;

namespace Tasky.Services.Identities.Application.Commands.AssignRoleToUserCommands;

public class AssignRoleToUserCommand :ICommand<Result>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}