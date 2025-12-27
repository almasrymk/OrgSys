namespace Application.Abstraction.Command
{
    using MediatR;
    using Domain.Shared;

    public interface ICommand : IRequest<Result>
    {

    }
     
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {

    }

    public interface ICommandOb<TResponse> : IRequest<Result<object>>
    {

    }


    public interface ICommandCollection<TResponse> : IRequest<ResultCollection<TResponse>>
    {

    }  
    
    public interface ICommandPagination<TResponse> : IRequest<ResultPagination<TResponse>>
    {

    }    
}