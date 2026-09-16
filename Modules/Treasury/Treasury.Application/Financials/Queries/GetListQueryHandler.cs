namespace Treasury.Application.Financials.Commands
{
    using Parties.Contracts.Dealers;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListFinancialQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<FinancialDto>, IListQuery<ResultCollection<FinancialDto>>;

    public sealed class GetListQueryHandler(IRepository<Treasury.Domain.Financial> _Repository, IMapper mapper, ISender sender) : ListCommandHandler<GetListFinancialQuery, Treasury.Domain.Financial, FinancialDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Financial, bool>> CreateFilter(GetListFinancialQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Currency,FinancialAccount,ContraFinancialAccount,FinancialType";
        }

        override public Func<IQueryable<Treasury.Domain.Financial>, IOrderedQueryable<Treasury.Domain.Financial>> CreateOrderBy(GetListFinancialQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override async Task<ResultCollection<FinancialDto>> Handle(GetListFinancialQuery request, CancellationToken cancellationToken)
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

            return result;
        }
    }
}
