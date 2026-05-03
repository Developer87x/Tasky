namespace Tasky.Services.Identities.Domain.SharedKernel;

public interface IEntity<IKentity> 
{
    IKentity Id { get; }
}


