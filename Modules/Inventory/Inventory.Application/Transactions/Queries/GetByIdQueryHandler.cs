namespace Inventory.Application.Transactions.Queries
{
    using Accounting.Contracts.Postings;
    using Catalog.Contracts.Products;
    using Catalog.Contracts.Units;
    using CommercialDocuments.Contracts.Invoices;
    using Parties.Contracts.Dealers;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdTransactionQuery(long Id) : ICommand<TransactionDto> , IGetByIdQuery<Result<TransactionDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Inventory.Domain.Transaction> _Repository, ISender sender, IMapper mapper) : GetCommandHandler<GetByIdTransactionQuery, Inventory.Domain.Transaction, TransactionDto>(_Repository, mapper)
    {
        public override async Task<Result<TransactionDto>> Handle(GetByIdTransactionQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response == null || result.Response.Id == 0)
                return result;

            var invoice = (await sender.Send(new GetInvoiceByLinkedTransactionQuery(result.Response.Id), cancellationToken)).Response;
            if (invoice != null)
            {
                result.Response.SourceInvoiceId = invoice.Id;
                result.Response.SourceInvoiceCode = invoice.Code;
                result.Response.SourceInvoiceTypeId = (long)invoice.TypeId;
            }

            var journal = (await sender.Send(new GetAccountingDocumentJournalQuery("transaction", result.Response.Id, result.Response.TypeId), cancellationToken)).Response;
            result.Response.JournalId = journal?.JournalId;
            result.Response.JournalCode = journal?.JournalCode;
            if (result.Response.DealerId is > 0)
            {
                var names = (await sender.Send(new GetDealerNamesQuery([result.Response.DealerId.Value]), cancellationToken)).Response ?? [];
                result.Response.DealerName = names.GetValueOrDefault(result.Response.DealerId.Value);
            }

            var lines = result.Response.TransactionProductList ?? [];
            if (lines.Count > 0)
            {
                var productIds = lines.Select(e => e.ProductId).Distinct().ToList();
                var unitIds = lines.Select(e => e.UnitId).Distinct().ToList();
                var productNames = (await sender.Send(new GetProductNamesQuery(productIds), cancellationToken)).Response ?? [];
                var unitNames = (await sender.Send(new GetUnitNamesQuery(unitIds), cancellationToken)).Response ?? [];
                var productUnits = (await sender.Send(new GetProductUnitsQuery(productIds), cancellationToken)).Response ?? [];
                foreach (var line in lines)
                {
                    line.ProductName = productNames.GetValueOrDefault(line.ProductId);
                    line.UnitName = unitNames.GetValueOrDefault(line.UnitId);
                    line.UnitList = productUnits.Where(u => u.ProductId == line.ProductId)
                        .Select(u => new UnitNameDto { Id = u.UnitId, Name = u.UnitName })
                        .ToList();
                }
            }
            return result;
        }

        public override string CreateInclude()
        {
            return "TransactionProducts";
        }

        public override Expression<Func<Inventory.Domain.Transaction, bool>> CreateFilter(GetByIdTransactionQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
