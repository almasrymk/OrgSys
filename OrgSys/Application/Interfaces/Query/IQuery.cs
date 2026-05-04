namespace Application.Abstraction.Query
{
    using MediatR;
    using Domain.Shared;

    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}