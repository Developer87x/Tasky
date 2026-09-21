using Tasky.BuildingBlocks.Core.Models;

namespace Tasky.Services.Projects.Domain.Enumerations;

public class ProjectStatus :Enumeration
{
    public ProjectStatus(int id, string name) : base(id, name)
    {
    }
    public static ProjectStatus Active => new(1, "Active");
    public static ProjectStatus Inactive => new(2, "Inactive");
    public static ProjectStatus Completed => new(3, "Completed");
    public static ProjectStatus Cancelled => new(4, "Cancelled");
    public static ProjectStatus Postponed => new(5, "Postponed");
    public static ProjectStatus Paused => new(6, "Paused");
    public static ProjectStatus Archived => new(7, "Archived");
    public static ProjectStatus Draft => new(8, "Draft");
}