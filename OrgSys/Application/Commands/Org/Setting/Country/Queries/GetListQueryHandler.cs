namespace Application.Commands.Org.Setting.Country.Queries
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

    public sealed record GetListCountryQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<CountryModelView> , IListQuery<ResultCollection<CountryModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Country> _Repository, IMapper mapper) : ListCommandHandler<GetListCountryQuery, Entity.Model.Country, CountryModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Country, bool>> CreateFilter(GetListCountryQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Country>, IOrderedQueryable<Entity.Model.Country>> CreateOrderBy(GetListCountryQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}