using Application.Abstraction.Command;
using Application.Commands.Org.Financials.Financial.Commands;
using Application.Common.Queries;
using Application.Interfaces.CQRS;
using AutoMapper;
using Domain.Abstraction;
using Domain.Shared;
using Domain.Entities;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace Application.Commands.Org.Invoices.Invoice.Queries
{


    public sealed record GetProductsNotReturnedQuery(long Id) : ICommandCollection<InvoiceProductModelView>, IGetByIdQuery<ResultCollection<InvoiceProductModelView>>;

    public sealed class GetProductsNotReturnedQueryHandler(IRepository<Domain.Entities.Invoice> _Repository, IMapper mapper) : ListCommandHandler<GetProductsNotReturnedQuery, Domain.Entities.Invoice, InvoiceProductModelView>(_Repository, mapper)
    {

        public override async Task<ResultCollection<InvoiceProductModelView>> Handle(GetProductsNotReturnedQuery request, CancellationToken cancellationToken)
        {

            var Invlist = await _Repository.GetListByFilterAsync(e => e.ParentId == request.Id, "InvoiceProducts");
            List<InvoiceProduct> proList = new List<InvoiceProduct>();
            foreach (var item in Invlist)
                proList.AddRange(item.InvoiceProducts);
            var ob = await _Repository.GetByFilterAsync( e => e.Id == request.Id, "InvoiceProducts");

            if (ob == null)
                ob = new() { InvoiceProducts = new List<InvoiceProduct>() };
            foreach (var item in ob.InvoiceProducts)
                item.Quantity -= proList.Where(e => e.ProductId == item.ProductId)?.Sum(e => e.Quantity) ?? 0;

            return new ResultCollection<InvoiceProductModelView>(
                          HttpStatusCode.OK,
                          ob.InvoiceProducts.Select(e => mapper.Map<InvoiceProductModelView>(e)).ToList(),
                          null);
        }

    }

}
