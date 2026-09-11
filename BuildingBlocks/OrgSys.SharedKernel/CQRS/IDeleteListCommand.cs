using MediatR;

namespace OrgSys.SharedKernel
{
    public interface IDeleteListCommand<TResponse> : IRequest<TResponse>
    {
        List<long> Ids { get; }
    }
}