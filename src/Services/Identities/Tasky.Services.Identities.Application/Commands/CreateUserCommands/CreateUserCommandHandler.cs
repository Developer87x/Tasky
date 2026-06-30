using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Common;
using Tasky.Services.Identities.Domain.Entities;
using Tasky.Services.Identities.Domain.Exceptions;
using Tasky.Services.Identities.Domain.Repositories;
using Tasky.Services.Identities.Domain.ValueObjects;

namespace Tasky.Services.Identities.Application.Commands.CreateUserCommands;

public class CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher hasher) : ICommandHandler<CreateUserCommand, Result>
{
    private readonly ILogger<CreateUserCommandHandler> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;
    private readonly IPasswordHasher _hasher = hasher;

    public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling CreateUserCommand for Email: {Email}, UserName: {UserName}", command.Email, command.UserName);
        
        // Check if user with the same email already exists
        var existingUser = await _userRepository.GetByUserNameAsync(command.UserName!, cancellationToken);
        if (existingUser != null)
        {
            _logger.LogWarning("User with UserName: {UserName} already exists.", command.UserName);
            return Result.Failure("User with the same UserName already exists.");
        }
        var password = await _hasher.HashPasswordAsync(command.Password!);
        var newUser = User.Create(Email.Create(command.Email!), command.UserName, new Password(password)) ?? throw new DomainException("User creation failed due to invalid data.");
        var userRole = await _roleRepository.GetByNameAsync("Users");
        if(userRole == null)
        {
            _logger.LogError("Default role 'Users' not found.");
            throw new DomainException("Default role 'Users' not found.");
        }
        
        
        newUser.AddRole(userRole);
        await _userRepository.AddAsync(newUser, cancellationToken);
        var result = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        _logger.LogInformation("User created successfully with UserId: {UserId}", newUser.Id.Value);
        return result ? Result.Success() : Result.Failure("User creation failed.");
    }
}