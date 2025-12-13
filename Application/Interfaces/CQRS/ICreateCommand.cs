using Domain.Shared;
using MediatR;

namespace Application.Interfaces.CQRS
{
    public interface ICreateCommand<TResponse> : IRequest<TResponse> { }
}