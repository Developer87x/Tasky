using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.CreateRoleCommands;


public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, ResultCommand<CreateRolecCommandResult>>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public Task<ResultCommand<CreateRolecCommandResult>> Handle(CreateRoleCommand command)
    {
        throw new NotImplementedException();
    }
}