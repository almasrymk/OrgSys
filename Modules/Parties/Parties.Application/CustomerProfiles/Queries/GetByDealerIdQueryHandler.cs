namespace Parties.Application.CustomerProfiles.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    /// <summary>Returns the Customer role for a Dealer, or an empty (Id = 0) DTO if the Dealer
    /// doesn't have that role — mirrors Organization.Application.OrganizationSettings' same
    /// "not found is not an error" GetCommandHandler reuse.</summary>
    public sealed record GetCustomerProfileByDealerIdQuery(long DealerId) : ICommand<CustomerProfileDto>;

    public sealed class GetByDealerIdQueryHandler(IRepository<Parties.Domain.CustomerProfile> _Repository, IMapper mapper) : GetCommandHandler<GetCustomerProfileByDealerIdQuery, Parties.Domain.CustomerProfile, CustomerProfileDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.CustomerProfile, bool>> CreateFilter(GetCustomerProfileByDealerIdQuery request)
        {
            return e => e.DealerId == request.DealerId && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
