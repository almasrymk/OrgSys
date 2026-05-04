namespace Application.Commands.Org.Setting.Classification.Queries
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

    public sealed record SearchClassificationQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<ClassificationModelView> ,ISearchQuery<ResultPagination<ClassificationModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.Classification> _Repository, IMapper mapper) : SearchCommandHandler<SearchClassificationQuery, Entity.Model.Classification, ClassificationModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Classification, bool>> CreateFilter(SearchClassificationQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Classification>, IOrderedQueryable<Entity.Model.Classification>> CreateOrderBy(SearchClassificationQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}