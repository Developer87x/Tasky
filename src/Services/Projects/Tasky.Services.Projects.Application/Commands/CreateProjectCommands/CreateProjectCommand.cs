using Tasky.BuildingBlocks.Constants;
using Tasky.BuildingBlocks.Core.CRQS;

namespace Tasky.Services.Projects.Application.Commands.CreateProjectCommands;

public class CreateProjectCommand:ICommand<Result>
{
    public string? ProjectName { get; set; }
    public string? ProjectCode { get; set; }
    public string? CreatedBy { get; set; }
    public int Status { get; set; }
    public string? CategoryId { get; set; }
} 