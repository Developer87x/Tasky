namespace Tasky.Services.Identities.Application.Dtos;

public class AssignRoleToUserResult
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}