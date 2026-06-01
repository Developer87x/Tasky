namespace Tasky.Services.Identities.Application.Dtos;

public class CreatePermissionResult
{
    public Guid PermissionId { get; set; }
}

public class AssignRoleToUserResult
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}