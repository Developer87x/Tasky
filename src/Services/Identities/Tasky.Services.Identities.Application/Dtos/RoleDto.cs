namespace Tasky.Services.Identities.Application.Dtos;

public class RoleDto
{
    public string? RoleName { get; set; }
    public List<PermissionDto>? Permissions { get; set; }
}

public class PermissionDto
{
    public string? PermissionName { get; set; }
}
public class PermissionResultDto
{
    public string? Permission { get; set; }
}