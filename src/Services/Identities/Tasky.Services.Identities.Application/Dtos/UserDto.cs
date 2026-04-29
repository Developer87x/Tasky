namespace Tasky.Services.Identities.Application.Dtos;

public class UserDto
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<RoleDto>? Roles { get; set; }
}
