namespace Tasky.Services.Identities.Application.Commands
{
    public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        Task<TResponse> Handle(TCommand command);
    }
}