namespace Inventory.Application.Transactions.Queries
{
    using Parties.Contracts.Dealers;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListTransactionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<TransactionDto>, IListQuery<ResultCollection<TransactionDto>>;

    public sealed class GetListQueryHandler(IRepository<Inventory.Domain.Transaction> _Repository, IMapper mapper, ISender sender) : ListCommandHandler<GetListTransactionQuery, Inventory.Domain.Transaction, TransactionDto>(_Repository, mapper)
    {
        public override Expression<Func<Inventory.Domain.Transaction, bool>> CreateFilter(GetListTransactionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0
               || e.TypeId == request.TypeId
               || (request.TypeId == 1 && e.TypeId == 5)
               || (request.TypeId == 2 && e.TypeId == 6)) &&
           e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Stock,ToStock";
        }

        override public Func<IQueryable<Inventory.Domain.Transaction>, IOrderedQueryable<Inventory.Domain.Transaction>> CreateOrderBy(GetListTransactionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override async Task<ResultCollection<TransactionDto>> Handle(GetListTransactionQuery request, CancellationToken cancellationToken)
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
