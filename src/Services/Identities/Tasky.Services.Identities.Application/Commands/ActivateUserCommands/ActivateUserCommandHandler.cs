using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Common;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.ActivateUserCommands;

public class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand, Result>
{
    private readonly ILogger<ActivateUserCommandHandler> _logger;
    private readonly IUserRepository _userRepository;

    public ActivateUserCommandHandler(IUserRepository userRepository ,ILogger<ActivateUserCommandHandler> logger)
    {
        _logger= logger;
        _userRepository =userRepository;
    }
    public async Task<Result> Handle(ActivateUserCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("activating user {UserId}",command.UserId);
        var user = await _userRepository.GetByIdAsync(command.UserId,cancellationToken);
        if(user == null)
        {
            _logger.LogWarning("user {UserId} not found",command.UserId);
            return Result.Failure($"user with id {command.UserId} not found");
        }
        if(user.IsActive)
        {
            _logger.LogWarning("user {UserId} is already active",command.UserId);
            return Result.Failure($"user with id {command.UserId} is already active");
        }
        user.Activate();
        await _userRepository.UpdateAsync(user,cancellationToken);
        var updateResult = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if(!updateResult)
        {
            _logger.LogError("failed to save changes when activating user {UserId}",command.UserId);
            return Result.Failure("failed to save changes");   
        }
        _logger.LogInformation("user {UserId} activated successfully",command.UserId);
        return Result.Success();
    }
}