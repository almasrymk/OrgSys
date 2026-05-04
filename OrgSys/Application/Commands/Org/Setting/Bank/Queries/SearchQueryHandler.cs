namespace Application.Commands.Org.Setting.Bank.Queries
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

    public sealed record SearchBankQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<BankModelView> ,ISearchQuery<ResultPagination<BankModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Bank> _Repository, IMapper mapper) : SearchCommandHandler<SearchBankQuery, Entity.Model.Bank, BankModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Bank, bool>> CreateFilter(SearchBankQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Bank>, IOrderedQueryable<Entity.Model.Bank>> CreateOrderBy(SearchBankQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}