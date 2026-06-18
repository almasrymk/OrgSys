namespace Application.Commands.Org.Transactions.Transaction.Queries
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

    public sealed record SearchTransactionQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<TransactionModelView> ,ISearchQuery<ResultPagination<TransactionModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Transaction> _Repository, IMapper mapper) : SearchCommandHandler<SearchTransactionQuery, Entity.Model.Transaction, TransactionModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Transaction, bool>> CreateFilter(SearchTransactionQuery request)
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
            return "Dealer,Stock";
        }

        override public Func<IQueryable<Entity.Model.Transaction>, IOrderedQueryable<Entity.Model.Transaction>> CreateOrderBy(SearchTransactionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}