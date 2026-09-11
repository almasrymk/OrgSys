namespace OrgSys.SharedKernel
{
    using MediatR;

    public interface IDoCommand : IRequest<bool>
    {

    }

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