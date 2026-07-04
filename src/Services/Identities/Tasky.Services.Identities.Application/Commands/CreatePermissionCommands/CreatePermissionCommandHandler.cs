using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Common;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.CreatePermissionCommands;

public class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand, Result>
{
    private readonly ILogger<CreatePermissionCommandHandler> _logger;
    private readonly IPermissionRepository _permissionRepository;

    public CreatePermissionCommandHandler(IPermissionRepository permissionRepository,ILogger<CreatePermissionCommandHandler>logger)
    {
        _permissionRepository =permissionRepository;
        _logger =logger;
    }

    public async Task<Result> Handle(CreatePermissionCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("");
        var isExistingPermission = await _permissionRepository.GetByNameAsync(command.Name!);
        if(isExistingPermission != null)
        {
            _logger.LogWarning("Permission with name: {PermissionName} already exists.", command.Name);
            return Result.Failure($"Permission with name: {command.Name} already exists.");
        }
        var permission = Permission.Create(command.Name!);
        await _permissionRepository.AddAsync(permission);
        var result =await  _permissionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        if (!result)
        {
            _logger.LogError("Failed to save permission with name: {PermissionName} to the database.", command.Name);
            return Result.Failure($"Failed to save permission with name: {command.Name} to the database.");
        }
        _logger.LogInformation("Permission with name: {PermissionName} created successfully.", command.Name);
        return Result.Success();

    }
}