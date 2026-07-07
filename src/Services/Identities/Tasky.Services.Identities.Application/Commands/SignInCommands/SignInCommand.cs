using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Common;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Application.Services;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.SignInCommands;

public class SignInCommand : ICommand<Result<SignInResult>>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class SignInCommandHander : ICommandHandler<SignInCommand, Result<SignInResult>>
{

    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<SignInCommandHander> _logger;
    

    public SignInCommandHander(IUserRepository userRepository, ITokenService tokenService, IPasswordHasher passwordHasher, ILogger<SignInCommandHander> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }


    public async Task<Result<SignInResult>> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email!, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("Sign-in failed: User with email {Email} not found.", command.Email);
            return Result<SignInResult>.Failure("Invalid email or password.");
        }
        if (!user.IsActive)
        {
            _logger.LogWarning("Sign-in failed: User with email {Email} is not active.", command.Email);
            return Result<SignInResult>.Failure("User account is not active.");
        }
        var isPasswordValid =await _passwordHasher.VerifyPasswordAsync( user.Password!.Value,command.Password!, cancellationToken);
        if (!isPasswordValid)
        {
            _logger.LogWarning("Sign-in failed: Invalid password for user with email {Email}.", command.Email);
            return Result<SignInResult>.Failure("Invalid email or password.");
        }
        var token = _tokenService.GenerateToken(user);
        var refreshToken= user.AddRefreshToken();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        _logger.LogInformation("User with email {Email} signed in successfully.", command.Email);
        return Result<SignInResult>.Success(new SignInResult
        {
            Token = token,
            RefreshToken = refreshToken.RawToken!
        });
    }
}

