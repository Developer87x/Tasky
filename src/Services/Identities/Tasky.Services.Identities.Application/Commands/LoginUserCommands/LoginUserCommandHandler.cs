using Tasky.Services.Identities.Application.Commands.LoginUserCommands;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Application.Services;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.LoginUserCOmmands;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenSerive
) : ICommandHandler<LoginUserCommand, ResultDto<LoginResultDto>>
{
    public async Task<ResultDto<LoginResultDto>> Handle(LoginUserCommand command)
    {
        var user = await userRepository.GetByUserNameAsync(command.UserName!) ?? 
            throw new UnauthorizedException("Invalid username or password.");

        if (!await passwordHasher.VerifyPasswordAsync(user.Password!, command.Password!))
            throw new UnauthorizedException("Invalid username or password.");

        var accessToken = tokenSerive.GenerateToken(user);
        var refreshToken = user.AddRefreshToken();
        await userRepository.UnitOfWork.SaveEntitiesAsync();
        return new ResultDto<LoginResultDto>
        {
            IsSuccess = true,
            Data = new LoginResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.RawToken
            }
        };
    }
}