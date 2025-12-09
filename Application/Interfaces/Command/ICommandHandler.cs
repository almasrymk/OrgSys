namespace Application.Abstraction.Command
{
    using Domain.Shared;
    using Entity.Model;
    using MediatR;

    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand , Result> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
    { 
    }
}