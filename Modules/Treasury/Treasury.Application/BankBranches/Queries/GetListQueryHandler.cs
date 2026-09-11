namespace Treasury.Application.BankBranches.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListBankBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<BankBranchDto> , IListQuery<ResultCollection<BankBranchDto>>;

    public sealed class GetListQueryHandler(IRepository<Treasury.Domain.BankBranch> _Repository, IMapper mapper) : ListCommandHandler<GetListBankBranchQuery, Treasury.Domain.BankBranch, BankBranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.BankBranch, bool>> CreateFilter(GetListBankBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (request.ParentId == 0 || e.BankId == request.ParentId) &&
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Treasury.Domain.BankBranch>, IOrderedQueryable<Treasury.Domain.BankBranch>> CreateOrderBy(GetListBankBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Bank,Country,City,District";
        }
    }
}