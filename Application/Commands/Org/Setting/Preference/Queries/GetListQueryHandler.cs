namespace Application.Commands.Org.Setting.Preference.Queries
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

    public sealed record GetListPreferenceQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<PreferenceModelView> , IListQuery<ResultCollection<PreferenceModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Preference> _Repository, IMapper mapper) : ListCommandHandler<GetListPreferenceQuery, Entity.Model.Preference, PreferenceModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Preference, bool>> CreateFilter(GetListPreferenceQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
             (string.IsNullOrEmpty(request.KeySearch) || e.Reference.Contains(request.KeySearch)) &&
            (request.ParentId == 0 || e.ParentId == request.ParentId) &&
            (request.TypeId == 0 || e.TypeId == request.TypeId) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Preference>, IOrderedQueryable<Entity.Model.Preference>> CreateOrderBy(GetListPreferenceQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}