namespace Inventory.Application.Transactions.Queries
{
    using Accounting.Contracts.Postings;
    using CommercialDocuments.Contracts.Invoices;
    using Parties.Contracts.Dealers;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record SearchTransactionQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<TransactionDto> ,ISearchQuery<ResultPagination<TransactionDto>>;

    public sealed class SearchQueryHandler(IRepository<Inventory.Domain.Transaction> _Repository, ISender sender, IMapper mapper) : SearchCommandHandler<SearchTransactionQuery, Inventory.Domain.Transaction, TransactionDto>(_Repository, mapper)
    {
        public override async Task<ResultPagination<TransactionDto>> Handle(SearchTransactionQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            var transactionIds = result.Response.Select(e => e.Id).ToList();
            if (transactionIds.Count == 0)
                return result;

            var invoices = (await sender.Send(new GetInvoicesByLinkedTransactionsQuery(transactionIds), cancellationToken)).Response ?? [];
            foreach (var transaction in result.Response)
            {
                if (invoices.TryGetValue(transaction.Id, out var invoice))
                {
                    transaction.SourceInvoiceId = invoice.Id;
                    transaction.SourceInvoiceCode = invoice.Code;
                    transaction.SourceInvoiceTypeId = (long)invoice.TypeId;
                }
            }

            var relatedIds = result.Response.Where(e => e.ParentId > 0).Select(e => e.ParentId).Distinct().ToList();
            if (relatedIds.Count > 0)
            {
                var relatedTransactions = await _Repository.GetListByFilterAsync(e => relatedIds.Contains(e.Id));
                foreach (var transaction in result.Response.Where(e => e.ParentId > 0))
                    transaction.ParentCode = relatedTransactions?.FirstOrDefault(e => e.Id == transaction.ParentId)?.Code;
            }

            var journals = (await sender.Send(new GetAccountingDocumentJournalsQuery("transaction", transactionIds), cancellationToken)).Response ?? [];
            foreach (var transaction in result.Response)
            {
                var journal = journals.GetValueOrDefault(transaction.Id);
                transaction.JournalId = journal?.JournalId;
                transaction.JournalCode = journal?.JournalCode;
            }

            var dealerIds = result.Response.Where(e => e.DealerId is > 0).Select(e => e.DealerId!.Value).Distinct().ToList();
            if (dealerIds.Count > 0)
            {
                var names = (await sender.Send(new GetDealerNamesQuery(dealerIds), cancellationToken)).Response ?? [];
                foreach (var transaction in result.Response)
                    if (transaction.DealerId is > 0)
                        transaction.DealerName = names.GetValueOrDefault(transaction.DealerId.Value);
            }
            return result;
        }

        public override Expression<Func<Inventory.Domain.Transaction, bool>> CreateFilter(SearchTransactionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
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

        override public Func<IQueryable<Inventory.Domain.Transaction>, IOrderedQueryable<Inventory.Domain.Transaction>> CreateOrderBy(SearchTransactionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
