using Microsoft.Extensions.Logging;
using Tasky.Services.Projects.Application.Common;
using Tasky.Services.Projects.Domain.DomainEvents;
using Tasky.Services.Projects.Domain.Entities;
using Tasky.Services.Projects.Domain.Repositories;

namespace Tasky.Services.Projects.Application.Commands.CreateCategoryCommands;

public class CreateCategoryCommand :ICommand<Result>
{
    public string? CategoryName { get; set; }
}


public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, ILogger<CreateCategoryCommandHandler> logger) : ICommandHandler<CreateCategoryCommand, Result>
{
    private readonly ILogger<CreateCategoryCommandHandler> _logger=logger;
    private readonly ICategoryRepository _categoryRepository=categoryRepository;
    public Task<Result> HandleAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var category = Category.Create(command.CategoryName!);
        throw new NotImplementedException();
    }
}