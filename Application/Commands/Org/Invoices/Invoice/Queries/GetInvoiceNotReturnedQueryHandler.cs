using Application.Abstraction.Command;
using Application.Common.Queries;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Application.DTOs;
using System.Net;
using Utility;

namespace Application.Commands.Org.Invoices.Invoice.Queries
{
    public sealed record GetInvoiceNotReturnedQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<InvoiceModelView>, ISearchQuery<ResultPagination<InvoiceModelView>>;

    public sealed class GetInvoiceNotReturnedQueryHandler(IRepository<Domain.Entities.Invoice> _Repository, IMapper mapper) : SearchCommandHandler<GetInvoiceNotReturnedQuery, Domain.Entities.Invoice, InvoiceModelView>(_Repository, mapper)
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
                    e.Status != Domain.Enums.Status.Deleted &&
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

        override public Func<IQueryable<Domain.Entities.Invoice>, IOrderedQueryable<Domain.Entities.Invoice>> CreateOrderBy(GetInvoiceNotReturnedQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
