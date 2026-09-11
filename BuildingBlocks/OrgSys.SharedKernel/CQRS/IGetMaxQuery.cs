using MediatR;

namespace OrgSys.SharedKernel
{
    public interface IGetMaxQuery<TResponse> : IRequest<TResponse>
    {
        long ParentId { get; }
        long TypeId { get; }
    }
}