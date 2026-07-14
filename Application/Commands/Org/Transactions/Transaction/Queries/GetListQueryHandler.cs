namespace Application.Commands.Org.Setting.Transaction.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListTransactionQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<TransactionDto>, IListQuery<ResultCollection<TransactionDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Transaction> _Repository, IMapper mapper) : ListCommandHandler<GetListTransactionQuery, Domain.Entities.Transaction, TransactionDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Transaction, bool>> CreateFilter(GetListTransactionQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Code.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.Transaction>, IOrderedQueryable<Domain.Entities.Transaction>> CreateOrderBy(GetListTransactionQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}