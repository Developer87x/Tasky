using Microsoft.Extensions.Logging;
using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;

using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.CreateRoleCommands;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Result>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<CreateRoleCommandHandler> _logger;
    public CreateRoleCommandHandler(IRoleRepository roleRepository, ILogger<CreateRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }
    public async Task<Result> HandleAsync(CreateRoleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling CreateRoleCommand for RoleName: {RoleName}", command.RoleName);
        var existingRole = await _roleRepository.GetByNameAsync(command.RoleName!, cancellationToken);
        if (existingRole != null)
        {
            _logger.LogWarning("Role with RoleName: {RoleName} already exists.", command.RoleName);
            return Result.Failure("Role with the same name already exists.");
        }
        var newRole = Role.Create(command.RoleName!);
        await _roleRepository.AddAsync(newRole, cancellationToken);
        var result = await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        _logger.LogInformation("Role created successfully with RoleId: {RoleId}", newRole.Id.Value);
        return result ? Result.Success() : Result.Failure("Role creation failed.");
    }
}