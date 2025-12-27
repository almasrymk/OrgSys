namespace Application.Commands.Org.Invoices.Invoice.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Commands;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.Model;
    using Entity.ModelView;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxInvoiceQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<Entity.Model.Invoice> _Repository) : GetMaxCommandHandler<GetMaxInvoiceQuery, Entity.Model.Invoice>(_Repository)
    {
        public override Expression<Func<Invoice, bool>> CreateFilter(GetMaxInvoiceQuery request)
        {
            return e=>e.TypeId == request.TypeId;
        }

        public override Expression<Func<Invoice, object>> CreateSelector()
        {
            return e => e.CodeNumber;
        }
    }
}