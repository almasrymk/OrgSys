namespace Application.Commands.Org.Setting.Dealer.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListDealerQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<DealerDto> , IListQuery<ResultCollection<DealerDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Dealer> _Repository, IMapper mapper) : ListCommandHandler<GetListDealerQuery, Domain.Entities.Dealer, DealerDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Dealer, bool>> CreateFilter(GetListDealerQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.TypeId == request.TypeId &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Dealer>, IOrderedQueryable<Domain.Entities.Dealer>> CreateOrderBy(GetListDealerQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}