using Domain.Shared;
using MediatR;

namespace Application.Interfaces.CQRS
{
    public interface IDeleteCommand<TResponse> : IRequest<TResponse>
    {
        long Id { get; }
    }
}