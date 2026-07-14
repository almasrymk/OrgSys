namespace Application.Commands.Org.Setting.Outlay.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListOutlayQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<OutlayDto> , IListQuery<ResultCollection<OutlayDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Outlay> _Repository, IMapper mapper) : ListCommandHandler<GetListOutlayQuery, Domain.Entities.Outlay, OutlayDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Outlay, bool>> CreateFilter(GetListOutlayQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Outlay>, IOrderedQueryable<Domain.Entities.Outlay>> CreateOrderBy(GetListOutlayQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}