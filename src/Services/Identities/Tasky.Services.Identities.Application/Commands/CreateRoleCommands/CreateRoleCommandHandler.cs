using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.CreateRoleCommands
{
    public class CreateRoleCommandHandler(IRoleRepository roleRepository) : ICommandHandler<CreateRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository = roleRepository;

        public async Task<bool> Handle(CreateRoleCommand command)
        {
            var role = await _roleRepository.GetByNameAsync(command.RoleName!);
            if (role is not null)
            {
                throw new NotFoundException($"Role with name {command.RoleName} already exists.");
            }
            role = Role.Create(command.RoleName);
            await _roleRepository.AddAsync(role);
            var result = await _roleRepository.UnitOfWork.SaveEntitiesAsync();
            return result;
        }
    }
}