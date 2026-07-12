namespace Tasky.Services.Projects.Domain.Entities;

public readonly record struct ProjectId(Guid Value)
{
    public static ProjectId New => new(Guid.NewGuid());
    public static ProjectId From(Guid value) => new(value);
}