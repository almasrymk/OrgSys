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

    public sealed record GetListPropertyQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<PropertyDto> , IListQuery<ResultCollection<PropertyDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Property> _Repository, IMapper mapper) : ListCommandHandler<GetListPropertyQuery, Domain.Entities.Property, PropertyDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Property, bool>> CreateFilter(GetListPropertyQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Property>, IOrderedQueryable<Domain.Entities.Property>> CreateOrderBy(GetListPropertyQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}