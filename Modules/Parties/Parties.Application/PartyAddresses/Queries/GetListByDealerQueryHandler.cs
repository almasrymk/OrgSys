namespace Parties.Application.PartyAddresses.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListByDealerPartyAddressQuery(long DealerId, string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<PartyAddressDto>, IListQuery<ResultCollection<PartyAddressDto>>;

    public sealed class GetListByDealerQueryHandler(IRepository<Parties.Domain.PartyAddress> _Repository, IMapper mapper) : ListCommandHandler<GetListByDealerPartyAddressQuery, Parties.Domain.PartyAddress, PartyAddressDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.PartyAddress, bool>> CreateFilter(GetListByDealerPartyAddressQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            e.DealerId == request.DealerId &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Parties.Domain.PartyAddress>, IOrderedQueryable<Parties.Domain.PartyAddress>> CreateOrderBy(GetListByDealerPartyAddressQuery request)
        {
            return q => q.OrderByDescending(e => e.IsPrimary).ThenBy(e => e.AddressType);
        }
    }
}
