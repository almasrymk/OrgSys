namespace Treasury.Application.FinancialTypes.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListFinancialTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) :
        ICommandCollection<FinancialTypeDto>, IListQuery<ResultCollection<FinancialTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Treasury.Domain.FinancialType> _Repository, IMapper mapper) : 
        ListCommandHandler<GetListFinancialTypeQuery, Treasury.Domain.FinancialType, FinancialTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Treasury.Domain.FinancialType, bool>> CreateFilter(GetListFinancialTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || ("" + e.Code).Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Treasury.Domain.FinancialType>, IOrderedQueryable<Treasury.Domain.FinancialType>> CreateOrderBy(GetListFinancialTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}