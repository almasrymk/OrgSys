namespace SaaS.Application.Tenants.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdTenantQuery(long Id) : ICommand<TenantDto>, IGetByIdQuery<Result<TenantDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Tenant> _Repository, IMapper mapper) : GetCommandHandler<GetByIdTenantQuery, Tenant, TenantDto>(_Repository, mapper)
    {
        public override Expression<Func<Tenant, bool>> CreateFilter(GetByIdTenantQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
