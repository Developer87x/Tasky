using Tasky.BuildingBlocks.Core.Models;
using Tasky.Services.Projects.Domain.DomainEvents;

namespace Tasky.Services.Projects.Domain.Entities;

public sealed class Project : AggregateRoot<ProjectId>
{
	public string? ProjectName { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public string? ProjectCode { get; private set; } = string.Empty;
	public bool IsActive { get; private set; } = true;
	public string? ProjectManagerId { get; private set; } = string.Empty;
	public CategoryId? CategoryId { get; private set; }
	public Category? Category { get; private set; }
	public int ProjectStatusId { get; private set; }
	private Project(ProjectId id) : base(id)
	{
	}
	private Project(int projectStatusId,string projectName, string? projectCode, CategoryId categoryId,string createdBy) : this(ProjectId.New)
	{
		 ProjectName = projectName;
		 ProjectCode = projectCode?? projectName[..3].ToUpper();
		 CategoryId = categoryId;
		 CreatedAt	= DateTime.UtcNow;
		 CreatedBy = createdBy;
		 ProjectStatusId = projectStatusId;
		 AddDomainEvent(new ProjectCreatedEvent(Id.Value));
	}
	
	public static Project Create(int projectStatusId ,string projectName, string categoryId, string createdBy,string? projectCode = null)
	{
		return new Project(projectStatusId,projectName, projectCode, Entities.CategoryId.From(Guid.Parse(categoryId)), createdBy);
	}
	
	public void AssignProjectManager(string projectManagerId)
	{
		ProjectManagerId = projectManagerId;
		AddDomainEvent(new ProjectManagerAssignedEvent(Id.Value, projectManagerId));
	}
	
}
