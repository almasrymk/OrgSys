using Application.Abstraction.Command;
using Application.Common.Queries;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace Application.Commands.Org.Invoices.Invoice.Queries
{
    public sealed record GetCreditAllByDealerIdQuery(string KeySearch, long TypeId, int Page, int PageSize, long ParentId
        , long DealerId, long CurrencyId, string Ids) :
        ICommandPagination<InvoiceModelView>, ISearchQuery<ResultPagination<InvoiceModelView>>;

    public sealed class GetCreditAllByDealerIdQueryHandler(IRepository<Domain.Entities.Invoice> _Repository, IMapper mapper) :
        SearchCommandHandler<GetCreditAllByDealerIdQuery, Domain.Entities.Invoice, InvoiceModelView>(_Repository, mapper)
    {

        public override async Task<ResultPagination<InvoiceModelView>> Handle(GetCreditAllByDealerIdQuery request, CancellationToken cancellationToken)
        {

            if (request.Ids != null)
            {
                var idsList = request.Ids.Split(",").Where(e => e != "").ToList();
                if (idsList == null)
                    idsList = new List<string>();

                var result = await _Repository.GetPaginationByFilterAsync(CreateFilter(request), CreateOrderBy(request), "", request.Page, request.PageSize);

                return new ResultPagination<InvoiceModelView>( HttpStatusCode.OK,
                result.Items.Select(e => mapper.Map<InvoiceModelView>(e)).ToList(),
                result.Page, result.PageSize, result.TotalPages, null);

            }


            return new ResultPagination<InvoiceModelView>(
                    HttpStatusCode.InternalServerError,
                    new List<InvoiceModelView>(), 0, 0, 0,
                    new List<Error> { new Error("Error") });
        }

        public override Func<IQueryable<Domain.Entities.Invoice>, IOrderedQueryable<Domain.Entities.Invoice>> CreateOrderBy(GetCreditAllByDealerIdQuery request)
        {
            return e => e.OrderByDescending(e => e.Id);
        }

        public override Expression<Func<Domain.Entities.Invoice, bool>> CreateFilter(GetCreditAllByDealerIdQuery request)
        {
            //return repo.GetList(e => !ids.Contains(e.Id.ToString()) && e.TypeId == TypeId && e.DealerId == dealerId && e.CurrencyId == currencyId && e.Credit > 0 && ("" + textSearch == "" || e.Code.Contains("" + textSearch) || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<InvoiceModelView>()).ToPagedList(page, pageSize);

            return e => !request.Ids.Contains(e.Id.ToString()) && e.TypeId == request.TypeId && e.DealerId == request.DealerId && e.CurrencyId == request.CurrencyId && e.Credit > 0 && (request.KeySearch == "" || e.Code.Contains(request.KeySearch) || e.Dealer.Name.Contains(request.KeySearch));
        }
    }

}
