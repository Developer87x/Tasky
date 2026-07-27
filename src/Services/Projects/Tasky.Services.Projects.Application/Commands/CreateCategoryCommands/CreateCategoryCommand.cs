using Tasky.Services.Projects.Application.Common;
using Tasky.Services.Projects.Domain.DomainEvents;

namespace Tasky.Services.Projects.Application.Commands.CreateCategoryCommands;

public class CreateCategoryCommand :ICommand<Result>
{
    public string? CategoryName { get; set; }
}
