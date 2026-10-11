namespace Tasky.Services.Identities.Application.Dtos;

public class RoleDto
{
    public Guid RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PermissionDto> Permissions { get; set; } = [];
}