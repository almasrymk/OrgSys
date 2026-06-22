namespace Application.Commands.Org.Setting.Property.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Utility;

    public sealed record SearchPropertyQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<PropertyModelView> ,ISearchQuery<ResultPagination<PropertyModelView>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Property> _Repository, IMapper mapper) : SearchCommandHandler<SearchPropertyQuery, Domain.Entities.Property, PropertyModelView>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Property, bool>> CreateFilter(SearchPropertyQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Property>, IOrderedQueryable<Domain.Entities.Property>> CreateOrderBy(SearchPropertyQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}