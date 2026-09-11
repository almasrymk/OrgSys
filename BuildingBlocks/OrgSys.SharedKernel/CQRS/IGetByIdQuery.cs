using MediatR;

namespace OrgSys.SharedKernel
{
    public interface IGetByIdQuery<TResponse> : IRequest<TResponse>
    {
        long Id { get; }
    }
}