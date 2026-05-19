using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.Repositories;
using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Application.Commands.UpdateUserCommands;

public class UpdateUserCommandHandler
(
    IUserRepository userRepository
) : ICommandHandler<UpdateUserCommand, ResultCommand<UpdateUserCommandResult>>
{
    
    public async Task<ResultCommand<UpdateUserCommandResult>> Handle(UpdateUserCommand command)
    {
        var user= await userRepository.GetByIdAsync(Guid.Parse(command.UserId)) ?? throw new NotFoundException("User not found.");
        if (!string.IsNullOrEmpty(command.Email))
        {
            user.UpdateEmail(Email.Create(command.Email));
        }
        await userRepository.UnitOfWork.SaveEntitiesAsync();
        return new ()
        {
            IsSuccess = true,
            Data = new ()
            {
                User = new UserDto
                {
                    UserName = user.UserName,
                    Email = user.Email?.Value,
                    Roles = [.. user.Roles.Select(r => new RoleDto
                    {
                        RoleName = r.RoleName
                    })]
                }
            }
        };
    }
}