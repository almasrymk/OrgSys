using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using OrgSys.SharedKernel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace CommercialDocuments.Application.Invoices.Queries
{


    public sealed record GetProductsNotReturnedQuery(long Id) : ICommandCollection<InvoiceProductDto>, IGetByIdQuery<ResultCollection<InvoiceProductDto>>;

    public sealed class GetProductsNotReturnedQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> _Repository, IMapper mapper) : ListCommandHandler<GetProductsNotReturnedQuery, CommercialDocuments.Domain.Invoice, InvoiceProductDto>(_Repository, mapper)
    {

        public override async Task<ResultCollection<InvoiceProductDto>> Handle(GetProductsNotReturnedQuery request, CancellationToken cancellationToken)
        {

            var Invlist = await _Repository.GetListByFilterAsync(e => e.ParentId == request.Id, "InvoiceProducts");
            List<InvoiceProduct> proList = new List<InvoiceProduct>();
            foreach (var item in Invlist!)
                proList.AddRange(item.InvoiceProducts!);
            var ob = await _Repository.GetByFilterAsync( e => e.Id == request.Id, "InvoiceProducts");

            if (ob == null)
                ob = new() { InvoiceProducts = new List<InvoiceProduct>() };
            foreach (var item in ob.InvoiceProducts!)
                item.Quantity -= proList.Where(e => e.ProductId == item.ProductId)?.Sum(e => e.Quantity) ?? 0;

            return new ResultCollection<InvoiceProductDto>(
                          HttpStatusCode.OK,
                          ob.InvoiceProducts.Select(e => mapper.Map<InvoiceProductDto>(e)).ToList(),
                          null);
        }

    }

}
