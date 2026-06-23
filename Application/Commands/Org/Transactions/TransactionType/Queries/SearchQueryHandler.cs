namespace Application.Commands.Org.Transactions.TransactionType.Queries
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

    public sealed record SearchTransactionTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<TransactionTypeDto> ,ISearchQuery<ResultPagination<TransactionTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.TransactionType> _Repository, IMapper mapper) : SearchCommandHandler<SearchTransactionTypeQuery, Domain.Entities.TransactionType, TransactionTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.TransactionType, bool>> CreateFilter(SearchTransactionTypeQuery request)
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
            return "";
        }
        override public Func<IQueryable<Domain.Entities.TransactionType>, IOrderedQueryable<Domain.Entities.TransactionType>> CreateOrderBy(SearchTransactionTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}