namespace Tasky.BuildingBlocks.Core.CRQS;

public interface ICommandDispatcher
{
    Task<TResponse> DispatchAsync<TResponse>(ICommand<TResponse> command,CancellationToken cancellationToken);
}
