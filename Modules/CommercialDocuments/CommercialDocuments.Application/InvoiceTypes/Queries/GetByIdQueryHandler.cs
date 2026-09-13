namespace CommercialDocuments.Application.InvoiceTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdInvoiceTypeQuery(long Id) : ICommand<InvoiceTypeDto> , IGetByIdQuery<Result<InvoiceTypeDto>>;

    public sealed class GetByIdQueryHandler(IRepository<CommercialDocuments.Domain.InvoiceType> _Repository, IMapper mapper) : GetCommandHandler<GetByIdInvoiceTypeQuery, CommercialDocuments.Domain.InvoiceType, InvoiceTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<CommercialDocuments.Domain.InvoiceType, bool>> CreateFilter(GetByIdInvoiceTypeQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}