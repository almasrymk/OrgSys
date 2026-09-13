namespace Parties.Application.Dealers.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListDealerQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DealerDto> , IListQuery<ResultCollection<DealerDto>>;

    public sealed class GetListQueryHandler(
        IRepository<Parties.Domain.Dealer> _Repository,
        IRepository<Accounting.Domain.Account> accountRepository,
        IMapper mapper) : ListCommandHandler<GetListDealerQuery, Parties.Domain.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Parties.Domain.Dealer, bool>> CreateFilter(GetListDealerQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.TypeId == request.TypeId &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Parties.Domain.Dealer>, IOrderedQueryable<Parties.Domain.Dealer>> CreateOrderBy(GetListDealerQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            // See SearchQueryHandler.CreateInclude for why Country/City/District were added.
            // Account was removed from this list (see the GeneralLedger migration report) —
            // AccountCode/AccountName are patched in below instead.
            return "DealerGroup,Country,City,District";
        }

        public override async Task<ResultCollection<DealerDto>> Handle(GetListDealerQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

            var accountIds = result.Response.Where(e => e.AccountId is > 0).Select(e => e.AccountId!.Value).Distinct().ToList();
            if (accountIds.Count > 0)
            {
                var accounts = (await accountRepository.GetListByFilterAsync(a => accountIds.Contains(a.Id)))?.ToDictionary(a => a.Id) ?? [];
                foreach (var dto in result.Response)
                    if (dto.AccountId is > 0 && accounts.TryGetValue(dto.AccountId.Value, out var account))
                    {
                        dto.AccountCode = account.Code;
                        dto.AccountName = account.Name;
                    }
            }

            return result;
        }
    }
}
