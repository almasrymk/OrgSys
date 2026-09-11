using MediatR;

namespace OrgSys.SharedKernel
{
    public interface IDeleteCommand<TResponse> : IRequest<TResponse>
    {
        long Id { get; }
    }
}