namespace Treasury.Application.BankBranches.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchBankBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<BankBranchDto> ,ISearchQuery<ResultPagination<BankBranchDto>>;

    public sealed class SearchQueryHandler(IRepository<Treasury.Domain.BankBranch> _Repository, IMapper mapper) : SearchCommandHandler<SearchBankBranchQuery, Treasury.Domain.BankBranch, BankBranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.BankBranch, bool>> CreateFilter(SearchBankBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Treasury.Domain.BankBranch>, IOrderedQueryable<Treasury.Domain.BankBranch>> CreateOrderBy(SearchBankBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Bank,Country,City,District";
        }
    }
}