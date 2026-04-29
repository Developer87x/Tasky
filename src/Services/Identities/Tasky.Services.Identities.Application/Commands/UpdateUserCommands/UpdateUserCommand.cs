using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Domain.Entities;

namespace Tasky.Services.Identities.Application.Commands.UpdateUserCommands;

public class UpdateUserCommand :ICommand<ResultDto<UpdateUserResultDto>>
{
    public string UserId { get; set; } = string.Empty;
    public string? Email { get; set; }
}
