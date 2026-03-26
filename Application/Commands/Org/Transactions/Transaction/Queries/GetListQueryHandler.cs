namespace Application.Commands.Org.Setting.Transaction.Queries
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

    public sealed record GetListTransactionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<TransactionModelView>, IListQuery<ResultCollection<TransactionModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Transaction> _Repository, IMapper mapper) : ListCommandHandler<GetListTransactionQuery, Entity.Model.Transaction, TransactionModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Transaction, bool>> CreateFilter(GetListTransactionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Entity.Model.Transaction>, IOrderedQueryable<Entity.Model.Transaction>> CreateOrderBy(GetListTransactionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}