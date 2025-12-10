namespace Application.Commands.Org.City.Queries
{
    using AutoMapper;
    using Entity.ModelView;
    using Domain.Abstraction;
    using System.Linq.Expressions;
    using Application.Abstraction.Command;
    using Utility;
    using Application.Common.Queries;

    public sealed record SearchCommand(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<CityModelView>;

    public sealed class SearchCommandHandler(IRepository<Entity.Model.City> _Repository, IMapper mapper) : SearchCommandHandler<SearchCommand, Entity.Model.City, CityModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.City, bool>> CreateFilter(SearchCommand request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
    }
}