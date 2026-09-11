namespace MasterData.Application.Classifications.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListClassificationQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ClassificationDto> , IListQuery<ResultCollection<ClassificationDto>>;

    public sealed class GetListQueryHandler(IRepository<MasterData.Domain.Classification> _Repository, IMapper mapper) : ListCommandHandler<GetListClassificationQuery, MasterData.Domain.Classification, ClassificationDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.Classification, bool>> CreateFilter(GetListClassificationQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<MasterData.Domain.Classification>, IOrderedQueryable<MasterData.Domain.Classification>> CreateOrderBy(GetListClassificationQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
