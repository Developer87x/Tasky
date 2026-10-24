namespace Tasky.Services.Projects.Application.Dtos;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public CategoryDto? Category { get; set; }

}
public class CategoryDto
{
    public string CategoryName { get; set; } = string.Empty;
}