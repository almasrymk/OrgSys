using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace Sales.Application.Invoices.Queries
{
    public sealed record GetCreditAllByDealerIdQuery(string KeySearch, long TypeId, int Page, int PageSize, long ParentId
        , long DealerId, long CurrencyId, string Ids) :
        ICommandPagination<InvoiceDto>, ISearchQuery<ResultPagination<InvoiceDto>>;

    public sealed class GetCreditAllByDealerIdQueryHandler(IRepository<Sales.Domain.Invoice> _Repository, IMapper mapper) :
        SearchCommandHandler<GetCreditAllByDealerIdQuery, Sales.Domain.Invoice, InvoiceDto>(_Repository, mapper)
    {

        public override async Task<ResultPagination<InvoiceDto>> Handle(GetCreditAllByDealerIdQuery request, CancellationToken cancellationToken)
        {

            if (request.Ids != null)
            {
                var idsList = request.Ids.Split(",").Where(e => e != "").ToList();
                if (idsList == null)
                    idsList = new List<string>();

                var result = await _Repository.GetPaginationByFilterAsync(CreateFilter(request), CreateOrderBy(request), "", request.Page, request.PageSize);

                return new ResultPagination<InvoiceDto>( HttpStatusCode.OK,
                result!.Items.Select(e => mapper.Map<InvoiceDto>(e)).ToList(),
                result.Page, result.PageSize, result.TotalPages, null);

            }


            return new ResultPagination<InvoiceDto>(
                    HttpStatusCode.InternalServerError,
                    new List<InvoiceDto>(), 0, 0, 0,
                    new List<Error> { new Error("Error") });
        }

        public override Func<IQueryable<Sales.Domain.Invoice>, IOrderedQueryable<Sales.Domain.Invoice>> CreateOrderBy(GetCreditAllByDealerIdQuery request)
        {
            return e => e.OrderByDescending(e => e.Id);
        }

        public override Expression<Func<Sales.Domain.Invoice, bool>> CreateFilter(GetCreditAllByDealerIdQuery request)
        {
            //return repo.GetList(e => !ids.Contains(e.Id.ToString()) && e.TypeId == TypeId && e.DealerId == dealerId && e.CurrencyId == currencyId && e.Credit > 0 && ("" + textSearch == "" || e.Code.Contains("" + textSearch) || e.Dealer.Name.Contains("" + textSearch)), e => e.OrderByDescending(e => e.Id), Includes, Utility.Status.New).Select(e => e.Map<InvoiceModelView>()).ToPagedList(page, pageSize);

            return e => !request.Ids.Contains(e.Id.ToString()) && e.TypeId == request.TypeId && e.DealerId == request.DealerId && e.CurrencyId == request.CurrencyId && e.Credit > 0 && (request.KeySearch == "" || e.Code!.Contains(request.KeySearch) || e.Dealer!.Name.Contains(request.KeySearch));
        }
    }

}
