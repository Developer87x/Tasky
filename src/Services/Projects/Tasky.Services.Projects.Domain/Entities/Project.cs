using Tasky.Services.Projects.Domain.DomainEvents;
using Tasky.Services.Projects.Domain.SharedKernel;

namespace Tasky.Services.Projects.Domain.Entities;

public sealed class Project : AggregateRoot<Project, ProjectId>
{
	private Project(ProjectId id) : base(id)
	{
	}

	public Project(ProjectId id, string projectName,string project) : this(id)
	{
		ProjectName = projectName;

		AddDomainEvent(new ProjectCreatedEvent(id.Value));
	}

	public string? ProjectName { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public string? ProjectCode { get; private set; } = string.Empty;
	public bool IsActive { get; private set; } = true;
	public string? ProjectManagerId { get; private set; } = string.Empty;

	public static Project Create(ProjectId id, string projectName,string projectManagerId)
	{
		return new Project(id, projectName, projectManagerId);
	}
	public void AssignToNewProjectManager(string newProjectManagerId){
		this.ProjectManagerId = newProjectManagerId;
		this.AddDomainEvent(new ProjectManagerAssignedEvent(this.Id.Value, newProjectManagerId));
	}
}
