namespace Accounting.Application.Accounts.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListAccountQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<AccountDto> , IListQuery<ResultCollection<AccountDto>>;

    public sealed class GetListQueryHandler(IRepository<Accounting.Domain.Account> _Repository, IMapper mapper) : ListCommandHandler<GetListAccountQuery, Accounting.Domain.Account, AccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.Account, bool>> CreateFilter(GetListAccountQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Accounting.Domain.Account>, IOrderedQueryable<Accounting.Domain.Account>> CreateOrderBy(GetListAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "AccountType";
        }
    }
}