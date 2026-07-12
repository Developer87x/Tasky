using Tasky.Services.Projects.Domain.DomainEvents;
using Tasky.Services.Projects.Domain.SharedKernel;

namespace Tasky.Services.Projects.Domain.Entities;

public class Project : AggregateRoot<Project, ProjectId>
{
	protected Project(ProjectId id) : base(id)
	{
	}

	protected Project(ProjectId id, string projectName) : this(id)
	{
		ProjectName = projectName;
		AddDomainEvent(new ProjectCreatedEvent(id.Value));
	}

	public string? ProjectName { get; private set; } = string.Empty;
    public string? Description { get; private set; } = string.Empty;
    public string? ProjectCode { get; private set; } = string.Empty;

	public static Project Create(ProjectId id, string projectName)
	{
		return new Project(id, projectName);
	}
}
