namespace Parties.Application.PartyContacts.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListByDealerPartyContactQuery(long DealerId, string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<PartyContactDto>, IListQuery<ResultCollection<PartyContactDto>>;

    public sealed class GetListByDealerQueryHandler(IRepository<Parties.Domain.PartyContact> _Repository, IMapper mapper) : ListCommandHandler<GetListByDealerPartyContactQuery, Parties.Domain.PartyContact, PartyContactDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.PartyContact, bool>> CreateFilter(GetListByDealerPartyContactQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            e.DealerId == request.DealerId &&
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Parties.Domain.PartyContact>, IOrderedQueryable<Parties.Domain.PartyContact>> CreateOrderBy(GetListByDealerPartyContactQuery request)
        {
            return q => q.OrderByDescending(e => e.IsPrimary).ThenBy(e => e.Name);
        }
    }
}
