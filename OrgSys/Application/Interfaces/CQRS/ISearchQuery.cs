using Domain.Shared;
using MediatR;

namespace Application.Interfaces.CQRS
{
    public interface ISearchQuery<TResponse> : IRequest<TResponse>
    {
        string KeySearch { get; }
        long ParentId { get; }
        long TypeId { get; }
        int Page { get; }
        int PageSize { get; }
    }
}