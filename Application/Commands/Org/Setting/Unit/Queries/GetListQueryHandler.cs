namespace Application.Commands.Org.Setting.Unit.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListUnitQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<UnitDto> , IListQuery<ResultCollection<UnitDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Unit> _Repository, IMapper mapper) : ListCommandHandler<GetListUnitQuery, Domain.Entities.Unit, UnitDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Unit, bool>> CreateFilter(GetListUnitQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Unit>, IOrderedQueryable<Domain.Entities.Unit>> CreateOrderBy(GetListUnitQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}