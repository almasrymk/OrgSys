namespace Accounting.Application.Journals.Queries
{
    using MasterData.Contracts.Currencies;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListJournalQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<JournalDto>, IListQuery<ResultCollection<JournalDto>>;

    public sealed class GetListQueryHandler(IRepository<Accounting.Domain.Journal> _Repository, IMapper mapper, ISender sender) : ListCommandHandler<GetListJournalQuery, Accounting.Domain.Journal, JournalDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.Journal, bool>> CreateFilter(GetListJournalQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Accounting.Domain.Journal>, IOrderedQueryable<Accounting.Domain.Journal>> CreateOrderBy(GetListJournalQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            // Currency was removed (see the Accounting DDD cleanup report) — CurrencyName is
            // patched in below instead, resolved through MasterData.Contracts.
            return "JournalType,FiscalYear,OriginalJournal,ReversalJournal";
        }

        public override async Task<ResultCollection<JournalDto>> Handle(GetListJournalQuery request, CancellationToken cancellationToken)
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
