using Microsoft.Extensions.Logging;
using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;
using Tasky.Services.Projects.Domain.Entities;
using Tasky.Services.Projects.Domain.Repositories;

namespace Tasky.Services.Projects.Application.Commands.CreateCategoryCommands;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ILogger<CreateCategoryCommandHandler> logger) : ICommandHandler<CreateCategoryCommand, Result>
{
    private readonly ILogger<CreateCategoryCommandHandler> _logger=logger;
    private readonly ICategoryRepository _categoryRepository=categoryRepository;
    public async Task<Result> HandleAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var isExistingCategory =await  _categoryRepository.GetByNameAsync(command.CategoryName!, cancellationToken);
        if(isExistingCategory is not null)
        {
            _logger.LogWarning("Category with name {CategoryName} already exists.", command.CategoryName);
            return Result.Failure($"Category with name {command.CategoryName} already exists.");
        }
        _logger.LogInformation("Creating new category with name {CategoryName}.", command.CategoryName);
        var category = Category.Create(command.CategoryName!,command.UserId!);
        await _categoryRepository.AddAsync(category, cancellationToken);
        var result = await _categoryRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return result ? Result.Success() : Result.Failure("Failed to create category.");
    }
}