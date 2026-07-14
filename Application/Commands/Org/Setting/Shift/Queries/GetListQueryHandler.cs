namespace Application.Commands.Org.Setting.Shift.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListShiftQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ShiftDto> , IListQuery<ResultCollection<ShiftDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Shift> _Repository, IMapper mapper) : ListCommandHandler<GetListShiftQuery, Domain.Entities.Shift, ShiftDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Shift, bool>> CreateFilter(GetListShiftQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Shift>, IOrderedQueryable<Domain.Entities.Shift>> CreateOrderBy(GetListShiftQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}