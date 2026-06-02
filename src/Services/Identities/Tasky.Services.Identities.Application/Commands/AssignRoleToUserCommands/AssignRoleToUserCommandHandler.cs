using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.AssignRoleToUserCommands;

public class AssignRoleToUserCommandHandler : ICommandHandler<AssignRoleToUserCommand, ResultCommand<AssignRoleToUserResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<AssignRoleToUserCommandHandler> _logger;
    public AssignRoleToUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository,ILogger<AssignRoleToUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<ResultCommand<AssignRoleToUserResult>> Handle(AssignRoleToUserCommand command)
    {
        _logger.LogInformation("Handling {CommandName} for UserId: {UserId} and RoleId: {RoleId}", nameof(AssignRoleToUserCommand), command.UserId, command.RoleId);
        var user =await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            _logger.LogWarning("User with Id {UserId} not found", command.UserId);
            return new()
            {
                IsSuccess = false,
                Message = "User not found"
            };
        }
        var role = await _roleRepository.GetByIdAsync(command.RoleId);
        if (role == null)        {
            _logger.LogWarning("Role with Id {RoleId} not found", command.RoleId);
            return new()
            {
                IsSuccess = false,
                Message = "Role not found"
            };
        }
        user.AddRole(role);
        await _userRepository.UpdateAsync(user);
        _logger.LogInformation("Role with Id {RoleId} assigned to User with Id {UserId} successfully", command.RoleId, command.UserId);
        return new()
        {            IsSuccess = true,
            Message = "Role assigned to user successfully",
            Data = new AssignRoleToUserResult
            {
                UserId = user.Id.Value,
                RoleId = role.Id.Value,
            }
        };
    }
}