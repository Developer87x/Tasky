namespace Tasky.Services.Identities.Application.Commands;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        dynamic handler = _serviceProvider.GetService(handlerType) ??
            throw new InvalidOperationException($"No handler found for command type {command.GetType()}");
        return await handler.Handle((dynamic)command);
    }
}