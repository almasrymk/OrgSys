namespace Application.Commands.Org.Financials.FinancialType.Commands
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

    public sealed record GetListFinancialTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) :
        ICommandCollection<FinancialTypeModelView>, IListQuery<ResultCollection<FinancialTypeModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.FinancialType> _Repository, IMapper mapper) : 
        ListCommandHandler<GetListFinancialTypeQuery, Entity.Model.FinancialType, FinancialTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.FinancialType, bool>> CreateFilter(GetListFinancialTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Entity.Model.FinancialType>, IOrderedQueryable<Entity.Model.FinancialType>> CreateOrderBy(GetListFinancialTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}