namespace OrgSys.SharedKernel
{
    using MediatR;

    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}