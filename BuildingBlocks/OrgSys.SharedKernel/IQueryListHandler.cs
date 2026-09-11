namespace OrgSys.SharedKernel
{
    using MediatR;

    public interface IQueryListHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
    {

    }
}