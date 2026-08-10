namespace Tasky.BuildingBlocks.Core.Models;

public interface IEntity<TId> :IEntity
{
    TId Id { get; set; }
}


public interface IEntity
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

}
