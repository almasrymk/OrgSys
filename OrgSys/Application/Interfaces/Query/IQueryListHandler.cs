namespace Application.Abstraction.Query
{
    using MediatR;
    using Domain.Shared;

    public interface IQueryListHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
    {

    }
}