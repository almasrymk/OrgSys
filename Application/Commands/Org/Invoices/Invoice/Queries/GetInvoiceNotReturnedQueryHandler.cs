using Application.Abstraction.Command;
using Application.Commands.Org.Setting.Invoice.Queries;
using Application.Common.Queries;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Entity.ModelView;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Utility;

namespace Application.Commands.Org.Invoices.Invoice.Queries
{
    public sealed record GetInvoiceNotReturnedQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandPagination<InvoiceModelView>, ISearchQuery<ResultPagination<InvoiceModelView>>;

    public sealed class GetInvoiceNotReturnedQueryHandler(IRepository<Entity.Model.Invoice> _Repository, IMapper mapper) : SearchCommandHandler<GetInvoiceNotReturnedQuery, Entity.Model.Invoice, InvoiceModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Invoice, bool>> CreateFilter(GetInvoiceNotReturnedQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) && request.TypeId == e.TypeId &&
           e.Status != Status.Deleted && e.Hide != true;
        }
        public override string CreateInclude()
        {
            return "";
        }

        override public Func<IQueryable<Entity.Model.Invoice>, IOrderedQueryable<Entity.Model.Invoice>> CreateOrderBy(GetInvoiceNotReturnedQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
