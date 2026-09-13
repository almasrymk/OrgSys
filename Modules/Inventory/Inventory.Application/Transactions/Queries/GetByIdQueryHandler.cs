namespace Inventory.Application.Transactions.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdTransactionQuery(long Id) : ICommand<TransactionDto> , IGetByIdQuery<Result<TransactionDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Inventory.Domain.Transaction> _Repository, IRepository<CommercialDocuments.Domain.Invoice> invoiceRepository, IRepository<Accounting.Domain.Journal> journalRepository, IMapper mapper) : GetCommandHandler<GetByIdTransactionQuery, Inventory.Domain.Transaction, TransactionDto>(_Repository, mapper)
    {
        public override async Task<Result<TransactionDto>> Handle(GetByIdTransactionQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response == null || result.Response.Id == 0)
                return result;

            var invoice = await invoiceRepository.GetByFilterAsync(e => e.TransactionId == result.Response.Id, string.Empty);
            if (invoice != null)
            {
                result.Response.SourceInvoiceId = invoice.Id;
                result.Response.SourceInvoiceCode = invoice.Code;
                result.Response.SourceInvoiceTypeId = invoice.TypeId;
            }

            var journal = await journalRepository.GetByFilterAsync(
                e => e.RefranceTable == "transaction"
                    && e.RefranceId == result.Response.Id
                    && e.RefranceTypeId == result.Response.TypeId,
                string.Empty);
            result.Response.JournalId = journal?.Id;
            result.Response.JournalCode = journal?.Code;
            return result;
        }

        public override string CreateInclude()
        {
            return "TransactionProducts,TransactionProducts.Product.ProductUnits.Unit,TransactionProducts.Product.ProductUnits";
        }

        public override Expression<Func<Inventory.Domain.Transaction, bool>> CreateFilter(GetByIdTransactionQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
