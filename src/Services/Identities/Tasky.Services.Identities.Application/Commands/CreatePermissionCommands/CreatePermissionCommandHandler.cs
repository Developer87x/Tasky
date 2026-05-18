using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.CreatePermissionCommands;

public class CreatePermissionCommandHandler : ICommandHandler<CreatePermissionCommand, ResultDto<PermissionResultDto>>
{
    private readonly IPermissionRepository _permissionRepository;

    public CreatePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<ResultDto<PermissionResultDto>> Handle(CreatePermissionCommand command)
    {
        var isPermissionExist = await _permissionRepository.GetByNameAsync(command.PermissionName!) ??
            throw new BadRequestException("Permission with the same name already exists.");
        var permission = Permission.Create(command.PermissionName!);
        await _permissionRepository.AddAsync(permission);
        await _permissionRepository.UnitOfWork.SaveEntitiesAsync();
        return new ResultDto<PermissionResultDto>
        {
            IsSuccess = true,
            Data = new PermissionResultDto
            {
                Permission = permission.PermissionName
            }
        };
    }
}