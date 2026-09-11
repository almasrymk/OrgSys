namespace Treasury.Application.Financials.Commands
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListFinancialQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<FinancialDto>, IListQuery<ResultCollection<FinancialDto>>;

    public sealed class GetListQueryHandler(IRepository<Treasury.Domain.Financial> _Repository, IMapper mapper) : ListCommandHandler<GetListFinancialQuery, Treasury.Domain.Financial, FinancialDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.Financial, bool>> CreateFilter(GetListFinancialQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code!.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Dealer,Currency,FinancialAccount,ContraFinancialAccount,FinancialType,Journal";
        }

        override public Func<IQueryable<Treasury.Domain.Financial>, IOrderedQueryable<Treasury.Domain.Financial>> CreateOrderBy(GetListFinancialQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}