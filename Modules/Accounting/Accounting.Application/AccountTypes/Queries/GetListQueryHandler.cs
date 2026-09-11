namespace Accounting.Application.AccountTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListAccountTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<AccountTypeDto> , IListQuery<ResultCollection<AccountTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Accounting.Domain.AccountType> _Repository, IMapper mapper) : ListCommandHandler<GetListAccountTypeQuery, Accounting.Domain.AccountType, AccountTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Accounting.Domain.AccountType, bool>> CreateFilter(GetListAccountTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Accounting.Domain.AccountType>, IOrderedQueryable<Accounting.Domain.AccountType>> CreateOrderBy(GetListAccountTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}