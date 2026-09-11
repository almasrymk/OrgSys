namespace MasterData.Application.Units.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<UnitDto> , IListQuery<ResultCollection<UnitDto>>;

    public sealed class GetListQueryHandler(IRepository<MasterData.Domain.Unit> _Repository, IMapper mapper) : ListCommandHandler<GetListUnitQuery, MasterData.Domain.Unit, UnitDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.Unit, bool>> CreateFilter(GetListUnitQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<MasterData.Domain.Unit>, IOrderedQueryable<MasterData.Domain.Unit>> CreateOrderBy(GetListUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
