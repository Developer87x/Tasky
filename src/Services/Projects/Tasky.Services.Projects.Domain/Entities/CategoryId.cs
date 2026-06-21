namespace Tasky.Services.Projects.Domain.Entities;

public readonly record struct CategoryId(Guid Value)
{
    public static CategoryId New => new(Guid.NewGuid());
    public static CategoryId From(Guid value) => new(value);
}