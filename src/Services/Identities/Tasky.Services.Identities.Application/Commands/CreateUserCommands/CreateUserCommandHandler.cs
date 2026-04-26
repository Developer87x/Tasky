using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.Repositories;
using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Application.Commands.CreateUserCommands
{
    public class CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher passwordHasher) : ICommandHandler<CreateUserCommand, bool>
    {

        public async Task<bool> Handle(CreateUserCommand command)
        {
            var exists = await userRepository.IsEmailExistsAsync(command.Email!);
            if (exists)
                throw new BadRequestException("A user with this email or username already exists.");
            Password.Validate(command.Password!);
            var hashedPassword = await passwordHasher.HashPasswordAsync(command.Password!);
            var password = Password.FromHash(hashedPassword);
            var user = User.Create(command.Email, command.UserName, password);
            var role = await roleRepository.GetByNameAsync("Users");
            if (role is not null)
            {
                user.AddRole(role);
            }

            await userRepository.AddAsync(user);
            return await userRepository.UnitOfWork.SaveEntitiesAsync();

        }



    }
}