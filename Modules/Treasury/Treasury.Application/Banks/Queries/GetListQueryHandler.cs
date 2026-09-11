namespace Treasury.Application.Banks.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListBankQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<BankDto> , IListQuery<ResultCollection<BankDto>>;

    public sealed class GetListQueryHandler(IRepository<Treasury.Domain.Bank> _Repository, IMapper mapper) : ListCommandHandler<GetListBankQuery, Treasury.Domain.Bank, BankDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Bank, bool>> CreateFilter(GetListBankQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Treasury.Domain.Bank>, IOrderedQueryable<Treasury.Domain.Bank>> CreateOrderBy(GetListBankQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}