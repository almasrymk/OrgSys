namespace MasterData.Application.ReferenceTypes.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListReferenceTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ReferenceTypeDto> , IListQuery<ResultCollection<ReferenceTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<MasterData.Domain.ReferenceType> _Repository, IMapper mapper) : ListCommandHandler<GetListReferenceTypeQuery, MasterData.Domain.ReferenceType, ReferenceTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.ReferenceType, bool>> CreateFilter(GetListReferenceTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<MasterData.Domain.ReferenceType>, IOrderedQueryable<MasterData.Domain.ReferenceType>> CreateOrderBy(GetListReferenceTypeQuery request)
        {
            return q => q.OrderBy(e => e.Id);
        }
    }
}
