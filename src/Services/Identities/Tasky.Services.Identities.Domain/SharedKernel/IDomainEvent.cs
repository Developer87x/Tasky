namespace Tasky.Services.Identities.Domain.SharedKernel;

public interface IDomainEvent
{
    DateTime DateOccurred { get; }
}