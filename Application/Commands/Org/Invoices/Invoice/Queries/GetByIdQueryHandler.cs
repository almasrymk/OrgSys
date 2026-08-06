namespace Application.Commands.Org.Invoices.Invoice.Queries
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

    public sealed record GetByIdInvoiceQuery(long Id) : ICommand<InvoiceDto> , IGetByIdQuery<Result<InvoiceDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Domain.Entities.Invoice> _Repository, IRepository<Domain.Entities.Journal> journalRepository, IMapper mapper) : GetCommandHandler<GetByIdInvoiceQuery, Domain.Entities.Invoice, InvoiceDto>(_Repository, mapper)
    {
        public override async Task<Result<InvoiceDto>> Handle(GetByIdInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response == null)
                return result;

            var journal = await journalRepository.GetByFilterAsync(
                e => e.RefranceTable == "invoice"
                    && e.RefranceId == result.Response.Id
                    && e.RefranceTypeId == result.Response.TypeId,
                string.Empty);
            result.Response.JournalId = journal?.Id;
            result.Response.JournalCode = journal?.Code;
            return result;
        }

        public override string CreateInclude()
        {
            return "InvoiceProducts,InvoiceProducts.Product.ProductUnits.Unit,Transaction";
        }

        public override Expression<Func<Domain.Entities.Invoice, bool>> CreateFilter(GetByIdInvoiceQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Domain.Enums.Status.Deleted && e.Hide != true;
        }
    }
}
