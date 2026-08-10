using Tasky.BuildingBlocks.Core.Models;
using Tasky.Services.Projects.Domain.DomainEvents;

namespace Tasky.Services.Projects.Domain.Entities;

public sealed class Project : AggregateRoot<ProjectId>
{
	private Project(ProjectId id) : base(id)
	{
	}

	public Project(ProjectId id, string projectName,string project) : this(id)
	{
		ProjectName = projectName;
		CreatedAt = DateTime.UtcNow;

		AddDomainEvent(new ProjectCreatedEvent(id.Value));
	}

	public string? ProjectName { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public string? ProjectCode { get; private set; } = string.Empty;
	public bool IsActive { get; private set; } = true;
	public string? ProjectManagerId { get; private set; } = string.Empty;
	public CategoryId? CategoryId { get; private set; }
	public Category? Category { get; private set; }

	public static Project Create(ProjectId id, string projectName,string projectManagerId)
	{
		return new Project(id, projectName, projectManagerId);
	}
	public void AssignToNewProjectManager(string newProjectManagerId){
		this.ProjectManagerId = newProjectManagerId;
		this.AddDomainEvent(new ProjectManagerAssignedEvent(this.Id.Value, newProjectManagerId));
	}
}
