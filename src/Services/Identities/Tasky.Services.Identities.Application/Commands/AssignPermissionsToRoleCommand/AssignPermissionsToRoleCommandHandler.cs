using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Common;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.AssignPermissionsToRoleCommand;

public class AssignPermissionsToRoleCommandHandler : ICommandHandler<AssignPermissionsToRoleCommand, Result>
{
    private readonly ILogger<AssignPermissionsToRoleCommandHandler> _logger;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public AssignPermissionsToRoleCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository, ILogger<AssignPermissionsToRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _logger = logger;
    }
    public async Task<Result> Handle(AssignPermissionsToRoleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Assigning permissions to role with Id {RoleId}", command.RoleId);
        if (command.PermissionIds is null || command.PermissionIds.Count == 0)
        {
            _logger.LogWarning("No permission IDs were provided for role with Id {RoleId}", command.RoleId);
            return Result.Failure("At least one permission ID must be provided");
        }

        var permissionIds = command.PermissionIds
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        if (permissionIds.Count != command.PermissionIds.Count)
        {
            _logger.LogWarning("Duplicate or empty permission IDs were provided for role with Id {RoleId}", command.RoleId);
        }

        if (permissionIds.Count == 0)
        {
            return Result.Failure("Permission IDs cannot be empty");
        }

        var role = await _roleRepository.GetByIdAsync(command.RoleId, cancellationToken);
        if (role is null)
        {
            _logger.LogWarning("Role with Id {RoleId} not found", command.RoleId);
            return Result.Failure($"Role with Id {command.RoleId} not found");
        }

        var permissions = await _permissionRepository.GetPermissionsByIdsAsync(permissionIds, cancellationToken);
        if (permissions.Count != permissionIds.Count)
        {
            _logger.LogWarning("Some permissions not found for the provided IDs");
            return Result.Failure("Some permissions not found for the provided IDs");
        }

        var alreadyAssignedPermissions = role.Permissions
            .Where(permission => permissionIds.Contains(permission.Id.Value))
            .ToList();

        if (alreadyAssignedPermissions.Any())
        {
            var alreadyAssignedPermissionNames = string.Join(", ", alreadyAssignedPermissions.Select(p => p.PermissionName));
            _logger.LogWarning("Permissions {PermissionNames} are already assigned to role with Id {RoleId}", alreadyAssignedPermissionNames, command.RoleId);
            return Result.Failure($"Permissions {alreadyAssignedPermissionNames} are already assigned to role");
        }

        foreach (var permission in permissions)
        {
            role.AssignPermissionToRole(permission);
        }

        await _roleRepository.UpdateAsync(role, cancellationToken);
        var result = await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (result)
        {
            _logger.LogInformation("Permissions assigned to role with Id {RoleId} successfully", command.RoleId);
            return Result.Success();
        }

        _logger.LogError("Failed to save changes while assigning permissions to role with Id {RoleId}", command.RoleId);
        return Result.Failure("Failed to save changes while assigning permissions to role");
    }
}