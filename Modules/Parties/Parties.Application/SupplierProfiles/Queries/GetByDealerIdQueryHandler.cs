namespace Parties.Application.SupplierProfiles.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetSupplierProfileByDealerIdQuery(long DealerId) : ICommand<SupplierProfileDto>;

    public sealed class GetByDealerIdQueryHandler(IRepository<Parties.Domain.SupplierProfile> _Repository, IMapper mapper) : GetCommandHandler<GetSupplierProfileByDealerIdQuery, Parties.Domain.SupplierProfile, SupplierProfileDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.SupplierProfile, bool>> CreateFilter(GetSupplierProfileByDealerIdQuery request)
        {
            return e => e.DealerId == request.DealerId && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
