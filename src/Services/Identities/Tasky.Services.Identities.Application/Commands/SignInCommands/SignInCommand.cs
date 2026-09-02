using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Identities.Application.Dtos;

namespace Tasky.Services.Identities.Application.Commands.SignInCommands;

public class SignInCommand : ICommand<Result<SignInResult>>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

