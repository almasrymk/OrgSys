using Application.Abstraction.Command;
using Application.Common.Queries;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Entity.ModelView;
using System.Net;
using Utility;

namespace Application.Commands.Org.Invoices.Invoice.Queries
{
    public sealed record GetInvoiceNotReturnedQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<InvoiceModelView>, ISearchQuery<ResultPagination<InvoiceModelView>>;

    public sealed class GetInvoiceNotReturnedQueryHandler(IRepository<Entity.Model.Invoice> _Repository, IMapper mapper) : SearchCommandHandler<GetInvoiceNotReturnedQuery, Entity.Model.Invoice, InvoiceModelView>(_Repository, mapper)
    {

        public override async Task<ResultPagination<InvoiceModelView>> Handle(GetInvoiceNotReturnedQuery request, CancellationToken cancellationToken)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            var returnedInvoiceIds = (await _Repository.GetListByFilterAsync(e => e.ParentId > 0, ""))!.Select(e => e.ParentId).ToList();

            var result = await _Repository.GetPaginationByFilterAsync(
                e =>
                    !returnedInvoiceIds.Contains(e.Id) &&
                    e.TypeId == request.TypeId &&
                    (string.IsNullOrEmpty(request.KeySearch) ||
                     e.Code.Contains(request.KeySearch)) &&
                    e.Id != e.ParentId &&
                    e.Status != Status.Deleted &&
                    e.Hide != true,
                CreateOrderBy(request),
                "",
                request.Page,
                request.PageSize);


            return new ResultPagination<InvoiceModelView>(
             HttpStatusCode.OK,
             result!.Items.Select(mapper.Map<InvoiceModelView>).ToList(),
             result.Page,
             result.PageSize,
             result.TotalPages,
             null);
        }

        override public Func<IQueryable<Entity.Model.Invoice>, IOrderedQueryable<Entity.Model.Invoice>> CreateOrderBy(GetInvoiceNotReturnedQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
