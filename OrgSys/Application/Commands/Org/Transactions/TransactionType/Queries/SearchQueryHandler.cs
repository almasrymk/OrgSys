namespace Application.Commands.Org.Transactions.TransactionType.Queries
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

    public sealed record SearchTransactionTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<TransactionTypeModelView> ,ISearchQuery<ResultPagination<TransactionTypeModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.TransactionType> _Repository, IMapper mapper) : SearchCommandHandler<SearchTransactionTypeQuery, Entity.Model.TransactionType, TransactionTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.TransactionType, bool>> CreateFilter(SearchTransactionTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "";
        }
        override public Func<IQueryable<Entity.Model.TransactionType>, IOrderedQueryable<Entity.Model.TransactionType>> CreateOrderBy(SearchTransactionTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}