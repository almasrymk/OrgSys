namespace Application.Commands.Org.Setting.Account.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;
    using Domain.Enums;

    public sealed record GetListAccountQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<AccountDto> , IListQuery<ResultCollection<AccountDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Account> _Repository, IMapper mapper) : ListCommandHandler<GetListAccountQuery, Domain.Entities.Account, AccountDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Account, bool>> CreateFilter(GetListAccountQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Account>, IOrderedQueryable<Domain.Entities.Account>> CreateOrderBy(GetListAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "AccountType";
        }
    }
}