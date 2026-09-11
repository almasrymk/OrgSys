namespace OrgSys.SharedKernel
{
    using MediatR;

    public interface IDoCommandHandler<TCommand> : IRequestHandler<TCommand, bool> where TCommand : IDoCommand
    {
    }

    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand , Result> where TCommand : ICommand
    {
    }

    public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> where TCommand : ICommand<TResponse>
    { 
    }

    public interface ICommandObHandler<TCommand, TResponse> : IRequestHandler<TCommand, object> where TCommand : ICommandOb<object>
    { 
    }

    public interface ICommandCollectionHandler<TCommand, TResponse> : IRequestHandler<TCommand, ResultCollection<TResponse>> where TCommand : ICommandCollection<TResponse>
    { 
    }

    public interface ICommandPaginationHandler<TCommand, TResponse> : IRequestHandler<TCommand, ResultPagination<TResponse>> where TCommand : ICommandPagination<TResponse>
    { 
    }
}