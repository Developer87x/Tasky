using Tasky.Services.Identities.Application.Dtos;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.Repositories;
using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Application.Commands.UpdateUserCommands;

public class UpdateUserCommand :ICommand<ResultDto<UpdateUserResultDto>>
{
    public string UserId { get; set; } = string.Empty;
    public string? Email { get; set; }
}

public class UpdateUserCommandHandler
(
    IUserRepository userRepository
) : ICommandHandler<UpdateUserCommand, ResultDto<UpdateUserResultDto>>
{
    
    public async Task<ResultDto<UpdateUserResultDto>> Handle(UpdateUserCommand command)
    {
        var user= await userRepository.GetByIdAsync(Guid.Parse(command.UserId)) ?? throw new NotFoundException("User not found.");
        if (!string.IsNullOrEmpty(command.Email))
        {
            user.UpdateEmail(Email.Create(command.Email));
        }
        await userRepository.UnitOfWork.SaveEntitiesAsync();
        return new ResultDto<UpdateUserResultDto>
        {
            IsSuccess = true,
            Data = new UpdateUserResultDto
            {
                User = new UserDto
                {
                    UserName = user.UserName,
                    Email = user.Email?.Value,
                    Roles = user.Roles.Select(r => new RoleDto
                    {
                        RoleName = r.RoleName
                    }).ToList()
                }
            }
        };
    }
}