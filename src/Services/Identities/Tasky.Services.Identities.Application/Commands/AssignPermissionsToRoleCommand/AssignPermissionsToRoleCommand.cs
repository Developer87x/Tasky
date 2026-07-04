using Tasky.Services.Identities.Application.Common;

namespace Tasky.Services.Identities.Application.Commands.AssignPermissionsToRoleCommand;

public class AssignPermissionsToRoleCommand :ICommand<Result>
{
    public Guid RoleId { get; set; }
    public List<Guid>? PermissionIds { get; set; }
}
