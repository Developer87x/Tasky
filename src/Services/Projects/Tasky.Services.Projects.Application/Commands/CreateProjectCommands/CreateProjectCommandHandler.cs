using Microsoft.Extensions.Logging;
using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Projects.Domain.Entities;
using Tasky.Services.Projects.Domain.Repositories;

namespace Tasky.Services.Projects.Application.Commands.CreateProjectCommands;

public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand, Result>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<CreateProjectCommandHandler> _logger;

    public CreateProjectCommandHandler(IProjectRepository projectRepository,
        ILogger<CreateProjectCommandHandler> logger)
    {
        _projectRepository = projectRepository;
        _logger = logger;
    }

    public Task<Result> HandleAsync(CreateProjectCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}