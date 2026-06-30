using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Common;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.CreateRoleCommands;

public class CreateRoleCommand :ICommand<Result>
{
    public string? RoleName { get; set; }
}

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, Result>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<CreateRoleCommandHandler> _logger;
    public CreateRoleCommandHandler(IRoleRepository roleRepository, ILogger<CreateRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }
    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateRoleCommand for RoleName: {RoleName}", request.RoleName);
        var existingRole = await _roleRepository.GetByNameAsync(request.RoleName!, cancellationToken);
        if (existingRole != null)
        {
            _logger.LogWarning("Role with RoleName: {RoleName} already exists.", request.RoleName);
            return Result.Failure("Role with the same name already exists.");
        }
        var newRole = Role.Create(request.RoleName!);
        await _roleRepository.AddAsync(newRole, cancellationToken);
        var result = await _roleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        _logger.LogInformation("Role created successfully with RoleId: {RoleId}", newRole.Id.Value);
        return result ? Result.Success() : Result.Failure("Role creation failed.");
    }
}