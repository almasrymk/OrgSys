namespace Application.Commands.Org.Setting.Currency.Queries
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

    public sealed record GetListCurrencyQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CurrencyModelView> , IListQuery<ResultCollection<CurrencyModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Currency> _Repository, IMapper mapper) : ListCommandHandler<GetListCurrencyQuery, Entity.Model.Currency, CurrencyModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Currency, bool>> CreateFilter(GetListCurrencyQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Currency>, IOrderedQueryable<Entity.Model.Currency>> CreateOrderBy(GetListCurrencyQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}