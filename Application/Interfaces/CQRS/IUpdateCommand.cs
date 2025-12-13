using Domain.Shared;
using MediatR;

namespace Application.Interfaces.CQRS
{
    public interface IUpdateCommand<TResponse> : IRequest<TResponse> { }
}
