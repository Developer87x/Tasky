using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;

namespace Tasky.Services.Projects.Application.Commands.CreateCategoryCommands;

public class CreateCategoryCommand :ICommand<Result>
{
    public string? CategoryName { get; set; }
    public string? UserId { get; set; }
}
