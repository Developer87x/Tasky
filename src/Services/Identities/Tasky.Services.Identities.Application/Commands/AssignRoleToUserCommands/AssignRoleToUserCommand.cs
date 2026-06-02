using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.AssignRoleToUserCommands;

public class AssignRoleToUserCommand:ICommand<ResultCommand<AssignRoleToUserResult>>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }    
}
