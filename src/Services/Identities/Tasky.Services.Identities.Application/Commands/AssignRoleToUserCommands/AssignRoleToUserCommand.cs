using Microsoft.Extensions.Logging;
namespace Tasky.Services.Identities.Application.Commands.AssignRoleToUserCommands;

public class AssignRoleToUserCommand :ICommand<Result>
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}


public class AssignRoleToUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, ILogger<AssignRoleToUserCommandHandler> logger) : ICommandHandler<AssignRoleToUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly ILogger<AssignRoleToUserCommandHandler> _logger = logger;

    public async Task<Result> Handle(AssignRoleToUserCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Assigning role {RoleId} to user {UserId}", command.RoleId, command.UserId);

        var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
        if (role == null)
        {
            _logger.LogWarning("Role {RoleId} not found", command.RoleId);
            return Result.Failure($"Role with ID {command.RoleId} not found.");
        }
        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", command.UserId);
            return Result.Failure($"User with ID {command.UserId} not found.");
        }
        if (user.Roles.Any(r => r.Id == role.Id))
        {
            _logger.LogWarning("User {UserId} already has role {RoleId}", command.UserId, command.RoleId);
            return Result.Failure($"User with ID {command.UserId} already has role with ID {command.RoleId}.");
        }
        // Assign the role to the user
        user.AddRole(role);
        await _userRepository.UpdateAsync(user, cancellationToken);
        var updateResult = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (!updateResult)
        {
            _logger.LogError("Failed to save changes when assigning role {RoleId} to user {UserId}", command.RoleId, command.UserId);
            return Result.Failure("Failed to save changes.");
        }   
        _logger.LogInformation("Role {RoleId} assigned to user {UserId}", command.RoleId, command.UserId);
        return Result.Success();
    }
}