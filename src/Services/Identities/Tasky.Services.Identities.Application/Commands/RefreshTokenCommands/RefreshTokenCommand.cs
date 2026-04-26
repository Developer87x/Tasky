using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.RefreshTokenCommands
{
    public class RefreshTokenCommand :ICommand<LoginResultDto>
    {
        public string? Token { get; set; }
    }
}
