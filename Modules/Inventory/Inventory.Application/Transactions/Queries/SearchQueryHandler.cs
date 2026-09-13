namespace Inventory.Application.Transactions.Queries
{
    using Accounting.Contracts.Postings;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record SearchTransactionQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<TransactionDto> ,ISearchQuery<ResultPagination<TransactionDto>>;

    public sealed class SearchQueryHandler(IRepository<Inventory.Domain.Transaction> _Repository, IRepository<CommercialDocuments.Domain.Invoice> invoiceRepository, ISender sender, IMapper mapper) : SearchCommandHandler<SearchTransactionQuery, Inventory.Domain.Transaction, TransactionDto>(_Repository, mapper)
    {
        public override async Task<ResultPagination<TransactionDto>> Handle(SearchTransactionQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            var transactionIds = result.Response.Select(e => e.Id).ToList();
            if (transactionIds.Count == 0)
                return result;

            var invoices = await invoiceRepository.GetListByFilterAsync(e => e.TransactionId.HasValue && transactionIds.Contains(e.TransactionId.Value));
            foreach (var transaction in result.Response)
            {
                var invoice = invoices?.FirstOrDefault(e => e.TransactionId == transaction.Id);
                if (invoice != null)
                {
                    transaction.SourceInvoiceId = invoice.Id;
                    transaction.SourceInvoiceCode = invoice.Code;
                    transaction.SourceInvoiceTypeId = invoice.TypeId;
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
            return "Dealer,Stock,ToStock";
        }

        override public Func<IQueryable<Inventory.Domain.Transaction>, IOrderedQueryable<Inventory.Domain.Transaction>> CreateOrderBy(SearchTransactionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
