using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Identities.Application.Common;

namespace Tasky.Services.Identities.Application.Commands.AssignRoleToUserCommands;

public class AssignRoleToUserCommand :ICommand<Result>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}