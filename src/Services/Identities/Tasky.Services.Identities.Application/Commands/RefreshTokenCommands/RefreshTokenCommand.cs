using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Identities.Application.Common;
using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.RefreshTokenCommands;

public class RefreshTokenCommand : ICommand<Result<SignInResult>>
{
    public string? Token { get; set; }
}