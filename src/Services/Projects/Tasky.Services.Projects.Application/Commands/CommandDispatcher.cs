using Microsoft.Extensions.DependencyInjection;

namespace Tasky.Services.Projects.Application.Commands;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResult> DispatchAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        dynamic handler = _serviceProvider.GetRequiredService(handlerType);
        return await handler.Handle((dynamic)command, cancellationToken);
    }
}