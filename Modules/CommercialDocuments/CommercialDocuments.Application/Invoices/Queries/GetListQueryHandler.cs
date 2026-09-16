namespace CommercialDocuments.Application.Invoices.Queries
{
    using Parties.Contracts.Dealers;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListInvoiceQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<InvoiceDto>, IListQuery<ResultCollection<InvoiceDto>>;

    public sealed class GetListQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> _Repository, IMapper mapper, ISender sender) : ListCommandHandler<GetListInvoiceQuery, CommercialDocuments.Domain.Invoice, InvoiceDto>(_Repository, mapper)
    {
        public override Expression<Func<CommercialDocuments.Domain.Invoice, bool>> CreateFilter(GetListInvoiceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<CommercialDocuments.Domain.Invoice>, IOrderedQueryable<CommercialDocuments.Domain.Invoice>> CreateOrderBy(GetListInvoiceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override async Task<ResultCollection<InvoiceDto>> Handle(GetListInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            var dealerIds = result.Response.Select(e => e.DealerId).Distinct().ToList();
            if (dealerIds.Count > 0)
            {
                var names = (await sender.Send(new GetDealerNamesQuery(dealerIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    dto.DealerName = names.GetValueOrDefault(dto.DealerId);
            }

            return result;
        }
    }
}
