namespace Tasky.Services.Projects.Domain.SharedKernel;

public interface IDomainEvent
{
    Guid Id { get; }
    
}