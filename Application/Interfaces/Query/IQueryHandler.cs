namespace Application.Abstraction.Query
{
    using MediatR;
    using Domain.Shared;

    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
    {

    }    
}