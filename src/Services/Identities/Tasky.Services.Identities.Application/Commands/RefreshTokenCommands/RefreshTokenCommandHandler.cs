using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Application.Services;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.RefreshTokenCommands;

public class RefreshTokenCommandHandler
(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    ITokenService tokenService

) : ICommandHandler<RefreshTokenCommand, ResultCommand<LoginCommandResult>>
{

    public async Task<ResultCommand<LoginCommandResult>> Handle(RefreshTokenCommand command)
    {
        var refreshToken = await refreshTokenRepository.GetByTokenAsync(command.Token!);
        if (refreshToken == null || !refreshToken.IsActive)
            throw new UnauthorizedException("Invalid refresh token");
        var user = await userRepository.GetByIdAsync(refreshToken.UserId.Value) ?? throw new NotFoundException("User not found");
        refreshToken.Revoke();
        var newRefreshToken = user.AddRefreshToken();
        await refreshTokenRepository.UnitOfWork.SaveEntitiesAsync();
        return new()
        {
            IsSuccess = true,
            Data = new()
            {
                AccessToken = tokenService.GenerateToken(user),
                RefreshToken = newRefreshToken.Token
            }
        };
    }
}