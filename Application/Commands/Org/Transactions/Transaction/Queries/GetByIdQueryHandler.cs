namespace Application.Commands.Org.Transactions.Transaction.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetByIdTransactionQuery(long Id) : ICommand<TransactionDto> , IGetByIdQuery<Result<TransactionDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Transaction> _Repository, IRepository<Domain.Entities.Invoice> invoiceRepository, IMapper mapper) : GetCommandHandler<GetByIdTransactionQuery, Domain.Entities.Transaction, TransactionDto>(_Repository, mapper)
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
            return result;
        }

        public override string CreateInclude()
        {
            return "TransactionProducts,TransactionProducts.Product.ProductUnits.Unit,TransactionProducts.Product.ProductUnits";
        }

        public override Expression<Func<Domain.Entities.Transaction, bool>> CreateFilter(GetByIdTransactionQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
