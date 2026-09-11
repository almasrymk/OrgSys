using MediatR;

namespace OrgSys.SharedKernel
{
    public interface IListQuery<TResponse> : IRequest<TResponse>
    {
        string KeySearch { get; }
        long ParentId { get; }
        long TypeId { get; }
        int Page { get; }
        int PageSize { get; }
    }
}