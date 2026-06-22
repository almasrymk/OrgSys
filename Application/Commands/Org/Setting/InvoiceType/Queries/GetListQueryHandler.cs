namespace Application.Commands.Org.Setting.InvoiceType.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Utility;

    public sealed record GetListInvoiceTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<InvoiceTypeModelView> , IListQuery<ResultCollection<InvoiceTypeModelView>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.InvoiceType> _Repository, IMapper mapper) : ListCommandHandler<GetListInvoiceTypeQuery, Domain.Entities.InvoiceType, InvoiceTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.InvoiceType, bool>> CreateFilter(GetListInvoiceTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.InvoiceType>, IOrderedQueryable<Domain.Entities.InvoiceType>> CreateOrderBy(GetListInvoiceTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}