namespace Application.Commands.Org.Setting.Bank.Queries
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

    public sealed record SearchBankQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<BankModelView> ,ISearchQuery<ResultPagination<BankModelView>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Bank> _Repository, IMapper mapper) : SearchCommandHandler<SearchBankQuery, Domain.Entities.Bank, BankModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Bank, bool>> CreateFilter(SearchBankQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Bank>, IOrderedQueryable<Domain.Entities.Bank>> CreateOrderBy(SearchBankQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}