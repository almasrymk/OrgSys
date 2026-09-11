using MediatR;

namespace OrgSys.SharedKernel
{
    public interface ICreateCommand<TResponse> : IRequest<TResponse> { }
}