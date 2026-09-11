namespace Sales.Application.InvoiceTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdInvoiceTypeQuery(long Id) : ICommand<InvoiceTypeDto> , IGetByIdQuery<Result<InvoiceTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Sales.Domain.InvoiceType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdInvoiceTypeQuery, Sales.Domain.InvoiceType, InvoiceTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Sales.Domain.InvoiceType, bool>> CreateFilter(GetByIdInvoiceTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}