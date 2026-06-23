namespace Application.Commands.Org.Setting.Bank.Queries
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

    public sealed record GetListBankQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<BankDto> , IListQuery<ResultCollection<BankDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Bank> _Repository, IMapper mapper) : ListCommandHandler<GetListBankQuery, Domain.Entities.Bank, BankDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Bank, bool>> CreateFilter(GetListBankQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Bank>, IOrderedQueryable<Domain.Entities.Bank>> CreateOrderBy(GetListBankQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}