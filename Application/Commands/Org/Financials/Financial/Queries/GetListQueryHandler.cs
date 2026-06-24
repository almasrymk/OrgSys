namespace Application.Commands.Org.Financials.Financial.Commands
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

    public sealed record GetListFinancialQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<FinancialDto>, IListQuery<ResultCollection<FinancialDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Financial> _Repository, IMapper mapper) : ListCommandHandler<GetListFinancialQuery, Domain.Entities.Financial, FinancialDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Financial, bool>> CreateFilter(GetListFinancialQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.Financial>, IOrderedQueryable<Domain.Entities.Financial>> CreateOrderBy(GetListFinancialQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}