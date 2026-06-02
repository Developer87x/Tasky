using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Domain.Repositories;
using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Application.Commands.ChangePasswordCommands;

public class ChangePasswordCommandHandler(IUserRepository userRepository,IPasswordHasher passwordHasher,ILogger<ChangePasswordCommandHandler> logger) : ICommandHandler<ChangePasswordCommand, ResultCommand<ChangePasswordResult>>
{
    private readonly IUserRepository _userRepository =userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher; 
    private readonly ILogger<ChangePasswordCommandHandler> _logger=logger;
    public async Task<ResultCommand<ChangePasswordResult>> Handle(ChangePasswordCommand command)
    {
        _logger.LogInformation("Handling ChangePasswordCommand for user id: {UserId}", command.UserId);
        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            _logger.LogWarning("User with id {UserId} not found", command.UserId);
            return new ResultCommand<ChangePasswordResult>
            {
                IsSuccess = false,
                Message = "User not found"   
            };
        }
        if(!await _passwordHasher.VerifyPasswordAsync(user.Password!.Value, command.CurrentPassword!))
        {
            _logger.LogWarning("Current password is incorrect for user id: {UserId}", command.UserId);
            return new ResultCommand<ChangePasswordResult>
            {
                IsSuccess = false,
                Message = "Current password is incorrect"   
            };
        }
        Password.Validate(command.NewPassword!);
        var hashedPassword = await _passwordHasher.HashPasswordAsync(command.NewPassword!);
        var newPassword = Password.FromHash(hashedPassword);
        user.UpdatePassword(newPassword);
        await _userRepository.UpdateAsync(user);
        await _userRepository.UnitOfWork.SaveEntitiesAsync();
        return new()
        {
            IsSuccess = true,
            Message = "Password changed successfully",
            Data = new ChangePasswordResult
            {
                UserId = user.Id.ToString()     
            }
        };
    }
}