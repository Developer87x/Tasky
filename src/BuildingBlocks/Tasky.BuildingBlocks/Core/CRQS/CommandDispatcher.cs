

namespace Tasky.BuildingBlocks.Core.CRQS;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> DispatchAsync<TResponse>(ICommand<TResponse> command,
        CancellationToken cancellationToken)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        dynamic handler = _serviceProvider.GetService(handlerType) ??
                          throw new InvalidOperationException($"No handler found for command type {command.GetType()}");
        return await handler.HandleAsync((dynamic)command, cancellationToken);
    }
}