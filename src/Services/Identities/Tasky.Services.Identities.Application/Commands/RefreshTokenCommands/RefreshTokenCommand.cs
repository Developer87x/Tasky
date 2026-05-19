using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Application.Commands.RefreshTokenCommands;

public class RefreshTokenCommand :ICommand<ResultCommand<LoginCommandResult>>
{
    public string? Token { get; set; }
}
