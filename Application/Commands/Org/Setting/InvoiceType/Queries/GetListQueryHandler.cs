namespace Application.Commands.Org.Setting.InvoiceType.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;
    using Utility;

    public sealed record GetListInvoiceTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<InvoiceTypeModelView> , IListQuery<ResultCollection<InvoiceTypeModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.InvoiceType> _Repository, IMapper mapper) : ListCommandHandler<GetListInvoiceTypeQuery, Entity.Model.InvoiceType, InvoiceTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.InvoiceType, bool>> CreateFilter(GetListInvoiceTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.InvoiceType>, IOrderedQueryable<Entity.Model.InvoiceType>> CreateOrderBy(GetListInvoiceTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}