namespace Application.Commands.Org.Setting.BankBranch.Queries
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

    public sealed record SearchBankBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<BankBranchModelView> ,ISearchQuery<ResultPagination<BankBranchModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.BankBranch> _Repository, IMapper mapper) : SearchCommandHandler<SearchBankBranchQuery, Entity.Model.BankBranch, BankBranchModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.BankBranch, bool>> CreateFilter(SearchBankBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.BankBranch>, IOrderedQueryable<Entity.Model.BankBranch>> CreateOrderBy(SearchBankBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Bank,Country,City,District";
        }
    }
}