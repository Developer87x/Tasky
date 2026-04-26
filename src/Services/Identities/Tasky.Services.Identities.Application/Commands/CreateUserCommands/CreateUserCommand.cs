namespace Tasky.Services.Identities.Application.Commands.CreateUserCommands
{
    public class CreateUserCommand :ICommand<bool>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}