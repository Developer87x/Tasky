using Microsoft.Extensions.Logging;
using Tasky.Services.Identities.Application.Common;
using Tasky.Services.Identities.Domain.Repositories;

namespace Tasky.Services.Identities.Application.Commands.AssignPermissionsToRoleCommand;

public class AssignPermissionsToRoleCommandHandler : ICommandHandler<AssignPermissionsToRoleCommand, Result>
{
    private readonly ILogger<AssignPermissionsToRoleCommandHandler> _logger;
    private readonly IRoleRepository _roleRepository;

    public AssignPermissionsToRoleCommandHandler(IRoleRepository roleRepository ,ILogger<AssignPermissionsToRoleCommandHandler> logger)
    {
        _roleRepository =roleRepository;
        _logger = logger;
    }
    public Task<Result> Handle(AssignPermissionsToRoleCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}