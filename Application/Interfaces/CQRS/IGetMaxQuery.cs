using Domain.Shared;
using MediatR;

namespace Application.Interfaces.CQRS
{
    public interface IGetMaxQuery : IRequest<object>
    {
        long ParentId { get; }
        long TypeId { get; }
    }
}