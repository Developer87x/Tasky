using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.UpdateUserCommands;

public class UpdateUserEmailCommand :ICommand<ResultCommand<UpdateUserEmailResult>>
{
    public string UserId { get; set; } = string.Empty;
    public string? Email { get; set; }
}
