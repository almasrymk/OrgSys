using Domain.Shared;
using MediatR;

namespace Application.Interfaces.CQRS
{
    public interface IGetByIdQuery<TResponse> : IRequest<TResponse>
    {
        long Id { get; }
    }
}