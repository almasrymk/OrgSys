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

    public interface ICommandCollectionHandler<TCommand, TResponse> : IRequestHandler<TCommand, ResultCollection<TResponse>> where TCommand : ICommandCollection<TResponse>
    { 
    }

    public interface ICommandPaginationHandler<TCommand, TResponse> : IRequestHandler<TCommand, ResultPagination<TResponse>> where TCommand : ICommandPagination<TResponse>
    { 
    }
}