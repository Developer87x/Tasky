namespace Tasky.Services.Identities.Application.Commands.CreateRoleCommands
{
    public class CreateRoleCommand :ICommand<bool>
    {
        public string? RoleName { get;  set; }
    }
}
