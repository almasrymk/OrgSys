namespace Application.Commands.Org.Setting.Account.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    
    public sealed record SearchAccountQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<AccountModelView> ,ISearchQuery<ResultPagination<AccountModelView>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Account> _Repository, IMapper mapper) : SearchCommandHandler<SearchAccountQuery, Domain.Entities.Account, AccountModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Account, bool>> CreateFilter(SearchAccountQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Account>, IOrderedQueryable<Domain.Entities.Account>> CreateOrderBy(SearchAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "AccountType";
        }
    }
}