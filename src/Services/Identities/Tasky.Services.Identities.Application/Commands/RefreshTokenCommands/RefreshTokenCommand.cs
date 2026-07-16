using Tasky.Services.Identities.Application.Common;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Application.Services;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.RefreshTokenCommands;

public class RefreshTokenCommand : ICommand<Result<SignInResult>>
{
    public string? Token { get; set; }
}

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<SignInResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<Result<SignInResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(command.Token!);
        if (refreshToken == null || !refreshToken.IsActive)
        {
            return Result<SignInResult>.Failure("Invalid or expired refresh token.");
        }

        var user = await _userRepository.GetByIdAsync(refreshToken.UserId!.Value, cancellationToken);
        if (user == null)
        {
            return Result<SignInResult>.Failure("User not found.");
        }

        // Revoke the old refresh token
        refreshToken.Revoke();
        var newRefreshToken = user.AddRefreshToken(); // Add a new refresh token for the user
        var resutl = await _refreshTokenRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (!resutl)
        {
            return Result<SignInResult>.Failure("Failed to save changes.");
        }
        return Result<SignInResult>.Success(new SignInResult
        {
            Token = _tokenService.GenerateToken(user),
            RefreshToken = newRefreshToken.RawToken!
        });


    }

}
