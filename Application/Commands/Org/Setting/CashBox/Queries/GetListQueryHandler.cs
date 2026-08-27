namespace Application.Commands.Org.Setting.CashBox.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListCashBoxQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<CashBoxDto>, IListQuery<ResultCollection<CashBoxDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.CashBox> _Repository, IMapper mapper) : ListCommandHandler<GetListCashBoxQuery, Domain.Entities.CashBox, CashBoxDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.CashBox, bool>> CreateFilter(GetListCashBoxQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.CashBox>, IOrderedQueryable<Domain.Entities.CashBox>> CreateOrderBy(GetListCashBoxQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
