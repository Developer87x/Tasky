namespace Tasky.BuildingBlocks.Core.Models;

public class Entity<TId> : IEntity<TId>
{
    public TId Id { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}