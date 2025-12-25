namespace Application.Commands.Org.Invoices.Invoice.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;

    public sealed record GetByIdInvoiceQuery(long Id) : ICommand<InvoiceModelView> , IGetByIdQuery<Result<InvoiceModelView>>;

    public sealed class GetByIdQueryHandler(IRepository<Entity.Model.Invoice> _Repository, IMapper mapper) : GetCommandHandler<GetByIdInvoiceQuery, Entity.Model.Invoice, InvoiceModelView>(_Repository, mapper)
    {
        public override string CreateInclude()
        {
            return "InvoiceProducts,InvoiceProducts.Product.ProductUnits.Unit";
        }

        public override Expression<Func<Entity.Model.Invoice, bool>> CreateFilter(GetByIdInvoiceQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=Utility.Status.Deleted && e.Hide != true;
        }
    }
}