namespace Application.Commands.Org.Setting.TransactionType.Queries
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

    public sealed record GetListTransactionTypeQuery(string KeySearch, long ParentId, long TypeId, int Page, int PageSize) : ICommandCollection<TransactionTypeDto>, IListQuery<ResultCollection<TransactionTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.TransactionType> _Repository, IMapper mapper) : ListCommandHandler<GetListTransactionTypeQuery, Domain.Entities.TransactionType, TransactionTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.TransactionType, bool>> CreateFilter(GetListTransactionTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
           (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
           (request.ParentId == 0 || e.ParentId == request.ParentId) &&
           (request.TypeId == 0 || e.TypeId == request.TypeId) &&
           e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }

        override public Func<IQueryable<Domain.Entities.TransactionType>, IOrderedQueryable<Domain.Entities.TransactionType>> CreateOrderBy(GetListTransactionTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}