using MediatR;

namespace OrgSys.SharedKernel
{
    public interface IUpdateCommand<TResponse> : IRequest<TResponse> { }
}
