namespace Accounting.Application.Journals.Queries
{
    using MasterData.Contracts.Currencies;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record SearchJournalQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<JournalDto> ,ISearchQuery<ResultPagination<JournalDto>>;

    public sealed class SearchQueryHandler(IRepository<Accounting.Domain.Journal> _Repository, IMapper mapper, ISender sender) : SearchCommandHandler<SearchJournalQuery, Accounting.Domain.Journal, JournalDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.Journal, bool>> CreateFilter(SearchJournalQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            // Currency was removed (see the Accounting DDD cleanup report) — CurrencyName is
            // patched in below instead, resolved through MasterData.Contracts.
            return "JournalType,FiscalYear,OriginalJournal,ReversalJournal";
        }

        override public Func<IQueryable<Accounting.Domain.Journal>, IOrderedQueryable<Accounting.Domain.Journal>> CreateOrderBy(SearchJournalQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override async Task<ResultPagination<JournalDto>> Handle(SearchJournalQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);

            var currencyIds = result.Response.Select(e => e.CurrencyId).Distinct().ToList();
            if (currencyIds.Count > 0)
            {
                var names = (await sender.Send(new GetCurrencyNamesQuery(currencyIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    dto.CurrencyName = names.GetValueOrDefault(dto.CurrencyId);
            }

            return result;
        }
    }
}
