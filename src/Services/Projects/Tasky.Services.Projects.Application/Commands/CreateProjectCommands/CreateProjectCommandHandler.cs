using Microsoft.Extensions.Logging;
using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.BuildingBlocks.Core.Models;
using Tasky.Services.Projects.Domain.Entities;
using Tasky.Services.Projects.Domain.Enumerations;
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

    public async Task<Result> HandleAsync(CreateProjectCommand command, CancellationToken cancellationToken = default)
    {
        var isExistingProject = await _projectRepository.GetByProjectNameAsync(command.ProjectName!, cancellationToken);
        if (isExistingProject is not null)
        {
            _logger.LogWarning("Project with name {ProjectName} already exists", command.ProjectName);
            return Result.Failure($"Project with name {command.ProjectName} already exists.");
        }
        _logger.LogInformation("Creating new Project with name {ProjectName}", command.ProjectName);
        var newProject = Project.Create(ProjectStatus.Draft.Id, command.ProjectName!, command.CategoryId!,
            command.CreatedBy!);
        await _projectRepository.AddAsync(newProject, cancellationToken);
        var result = await _projectRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? Result.Success() : Result.Failure("Failed to create project");
    }
}