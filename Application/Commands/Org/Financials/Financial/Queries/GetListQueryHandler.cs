namespace Application.Commands.Org.Financials.Financial.Commands
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

    public sealed record GetListFinancialQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<FinancialModelView>, IListQuery<ResultCollection<FinancialModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Financial> _Repository, IMapper mapper) : ListCommandHandler<GetListFinancialQuery, Entity.Model.Financial, FinancialModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Financial, bool>> CreateFilter(GetListFinancialQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Entity.Model.Financial>, IOrderedQueryable<Entity.Model.Financial>> CreateOrderBy(GetListFinancialQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}