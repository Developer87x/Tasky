namespace Tasky.Services.Identities.Application.Dtos;

public class RoleDto
{
    public string? RoleName { get; set; }
    public List<PermissionDto>? Permissions { get; set; }
}
