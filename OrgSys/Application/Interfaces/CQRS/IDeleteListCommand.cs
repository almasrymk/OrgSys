using Domain.Shared;
using MediatR;

namespace Application.Interfaces.CQRS
{
    public interface IDeleteListCommand<TResponse> : IRequest<TResponse>
    {
        List<long> Ids { get; }
    }
}