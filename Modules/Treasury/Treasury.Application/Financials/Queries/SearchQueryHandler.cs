namespace Treasury.Application.Financials.Commands
{
    using MasterData.Contracts.Currencies;
    using MasterData.Contracts.Lookups;
    using Parties.Contracts.Dealers;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record SearchFinancialQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<FinancialDto> ,ISearchQuery<ResultPagination<FinancialDto>>;

    public sealed class SearchQueryHandler(IRepository<Treasury.Domain.Financial> _Repository, IMapper mapper, ISender sender) : SearchCommandHandler<SearchFinancialQuery, Treasury.Domain.Financial, FinancialDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Financial, bool>> CreateFilter(SearchFinancialQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) ||
             (e.Code != null && e.Code.Contains(request.KeySearch)) ||
             (e.Notes != null && e.Notes.Contains(request.KeySearch)) ||
             (e.ReferenceNumber != null && e.ReferenceNumber.Contains(request.KeySearch)) ||
             (e.FinancialAccount != null && e.FinancialAccount.Name.Contains(request.KeySearch))) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.FinancialTypeId == request.TypeId) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "FinancialAccount,ContraFinancialAccount,FinancialType";
        }

        override public Func<IQueryable<Treasury.Domain.Financial>, IOrderedQueryable<Treasury.Domain.Financial>> CreateOrderBy(SearchFinancialQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override async Task<ResultPagination<FinancialDto>> Handle(SearchFinancialQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            var dealerIds = result.Response.Where(e => e.DealerId is > 0).Select(e => e.DealerId!.Value).Distinct().ToList();
            if (dealerIds.Count > 0)
            {
                var names = (await sender.Send(new GetDealerNamesQuery(dealerIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    if (dto.DealerId is > 0)
                        dto.DealerName = names.GetValueOrDefault(dto.DealerId.Value);
            }

            var paymentTypeIds = result.Response.Select(e => e.PaymentTypeId).Distinct().ToList();
            if (paymentTypeIds.Count > 0)
            {
                var names = (await sender.Send(new GetPaymentTypeNamesQuery(paymentTypeIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    dto.PaymentTypeName = names.GetValueOrDefault(dto.PaymentTypeId);
            }

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
