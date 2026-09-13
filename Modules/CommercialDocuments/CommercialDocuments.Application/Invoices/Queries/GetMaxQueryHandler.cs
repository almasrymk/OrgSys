namespace CommercialDocuments.Application.Invoices.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System;
    using System.Linq.Expressions;

    public sealed record GetMaxInvoiceQuery(long TypeId , long ParentId) : ICommandOb<object> , IGetMaxQuery<object>;

    public sealed class GetMaxQueryHandler(IRepository<CommercialDocuments.Domain.Invoice> _Repository) : GetMaxCommandHandler<GetMaxInvoiceQuery, CommercialDocuments.Domain.Invoice>(_Repository)
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