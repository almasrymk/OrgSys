namespace Application.Commands.Org.Transactions.Transaction.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchTransactionQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<TransactionDto> ,ISearchQuery<ResultPagination<TransactionDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Transaction> _Repository, IMapper mapper) : SearchCommandHandler<SearchTransactionQuery, Domain.Entities.Transaction, TransactionDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Transaction, bool>> CreateFilter(SearchTransactionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
            (request.ParentId ==0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Dealer,Stock";
        }

        override public Func<IQueryable<Domain.Entities.Transaction>, IOrderedQueryable<Domain.Entities.Transaction>> CreateOrderBy(SearchTransactionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}