namespace Application.Commands.Org.Setting.TransactionType.Queries
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

    public sealed record GetListTransactionTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<TransactionTypeModelView>, IListQuery<ResultCollection<TransactionTypeModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.TransactionType> _Repository, IMapper mapper) : ListCommandHandler<GetListTransactionTypeQuery, Entity.Model.TransactionType, TransactionTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.TransactionType, bool>> CreateFilter(GetListTransactionTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Entity.Model.TransactionType>, IOrderedQueryable<Entity.Model.TransactionType>> CreateOrderBy(GetListTransactionTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}