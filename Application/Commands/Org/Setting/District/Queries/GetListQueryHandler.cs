namespace Application.Commands.Org.Setting.District.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Entity.ModelView;
    using System.Linq.Expressions;
    using Utility;

    public sealed record GetListDistrictQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DistrictModelView> , IListQuery<ResultCollection<DistrictModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.District> _Repository, IMapper mapper) : ListCommandHandler<GetListDistrictQuery, Entity.Model.District, DistrictModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.District, bool>> CreateFilter(GetListDistrictQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&           
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.District>, IOrderedQueryable<Entity.Model.District>> CreateOrderBy(GetListDistrictQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}