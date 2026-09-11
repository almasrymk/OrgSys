namespace MasterData.Application.Districts.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListDistrictQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DistrictDto> , IListQuery<ResultCollection<DistrictDto>>;

    public sealed class GetListQueryHandler(IRepository<MasterData.Domain.District> _Repository, IMapper mapper) : ListCommandHandler<GetListDistrictQuery, MasterData.Domain.District, DistrictDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.District, bool>> CreateFilter(GetListDistrictQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&           
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<MasterData.Domain.District>, IOrderedQueryable<MasterData.Domain.District>> CreateOrderBy(GetListDistrictQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "City,Country";
        }
    }
}
