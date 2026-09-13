using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using System.Net;

namespace CommercialDocuments.Application.Invoices.Queries
{
    public sealed record GetInvoiceNotReturnedQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<InvoiceDto>, ISearchQuery<ResultPagination<InvoiceDto>>;

    public sealed class GetInvoiceNotReturnedQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> _Repository, IMapper mapper) : SearchCommandHandler<GetInvoiceNotReturnedQuery, CommercialDocuments.Domain.Invoice, InvoiceDto>(_Repository, mapper)
    {

        public override async Task<ResultPagination<InvoiceDto>> Handle(GetInvoiceNotReturnedQuery request, CancellationToken cancellationToken)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            var returnedInvoiceIds = (await _Repository.GetListByFilterAsync(e => e.ParentId > 0, ""))!.Select(e => e.ParentId).ToList();

            var result = await _Repository.GetPaginationByFilterAsync(
                e =>
                    !returnedInvoiceIds.Contains(e.Id) &&
                    e.TypeId == request.TypeId &&
                    (string.IsNullOrEmpty(request.KeySearch) ||
                     e.Code!.Contains(request.KeySearch)) &&
                    e.Id != e.ParentId &&
                    e.Status != OrgSys.SharedKernel.Status.Deleted &&
                    e.Hide != true,
                CreateOrderBy(request),
                "",
                request.Page,
                request.PageSize);


            return new ResultPagination<InvoiceDto>(
             HttpStatusCode.OK,
             result!.Items.Select(mapper.Map<InvoiceDto>).ToList(),
             result.Page,
             result.PageSize,
             result.TotalPages,
             null);
        }

        override public Func<IQueryable<CommercialDocuments.Domain.Invoice>, IOrderedQueryable<CommercialDocuments.Domain.Invoice>> CreateOrderBy(GetInvoiceNotReturnedQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
