using Domain.Shared;
using MediatR;

namespace Application.Interfaces.CQRS
{
    public interface IGetMaxQuery<TResponse> : IRequest<TResponse>
    {
        long ParentId { get; }
        long TypeId { get; }
    }
}