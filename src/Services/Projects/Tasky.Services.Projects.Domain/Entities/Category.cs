using Tasky.Services.Projects.Domain.SharedKernel;

namespace Tasky.Services.Projects.Domain.Entities;

public class Category :AggregateRoot<Category, CategoryId>
{
	private readonly List<Project> _projects = [];
    protected Category(CategoryId id) : base(id)
    {
    }

	protected Category(CategoryId id, string categoryName) : this(id)
	{
		CategoryName = categoryName;
	}

	public string? CategoryName { get; private set; } = string.Empty;
   
	public static Category Create(string categoryName)
	{
		return new Category(CategoryId.New, categoryName);
	}

	public void UpdateCategoryName(string categoryName)
	{
		CategoryName = categoryName;
		UpdatedAt = DateTime.UtcNow;
	}

}
