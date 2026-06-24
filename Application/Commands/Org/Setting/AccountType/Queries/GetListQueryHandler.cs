namespace Application.Commands.Org.Setting.AccountType.Queries
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

    public sealed record GetListAccountTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<AccountTypeDto> , IListQuery<ResultCollection<AccountTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.AccountType> _Repository, IMapper mapper) : ListCommandHandler<GetListAccountTypeQuery, Domain.Entities.AccountType, AccountTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.AccountType, bool>> CreateFilter(GetListAccountTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.AccountType>, IOrderedQueryable<Domain.Entities.AccountType>> CreateOrderBy(GetListAccountTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}