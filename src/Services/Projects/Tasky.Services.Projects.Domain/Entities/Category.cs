using Tasky.BuildingBlocks.Core.Models;
using Tasky.Services.Projects.Domain.DomainEvents;

namespace Tasky.Services.Projects.Domain.Entities;

public class Category :AggregateRoot<CategoryId>
{
	private readonly List<Project> _projects = [];
    protected Category(CategoryId id) : base(id)
    {
    }

	protected Category(CategoryId id, string categoryName,string userId) : this(id)
	{
		CategoryName = categoryName;
		CreatedAt = DateTime.UtcNow;
		CreatedBy = userId;
		AddDomainEvent(new CategoryCreatedEvent(this.Id.Value));	
	}

	public string? CategoryName { get; private set; } = string.Empty;
	public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();
   
	public static Category Create(string categoryName,string userId)
	{
		return new Category(CategoryId.New, categoryName,userId);
	}

	public void UpdateCategoryName(string categoryName)
	{
		CategoryName = categoryName;
		LastModified = DateTime.UtcNow;
		AddDomainEvent(new CategoryUpdatedEvent(this.Id.Value));
	}

}
