namespace Application.Commands.Org.Setting.BankBranch.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record GetListBankBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<BankBranchDto> , IListQuery<ResultCollection<BankBranchDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.BankBranch> _Repository, IMapper mapper) : ListCommandHandler<GetListBankBranchQuery, Domain.Entities.BankBranch, BankBranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.BankBranch, bool>> CreateFilter(GetListBankBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (request.ParentId == 0 || e.BankId == request.ParentId) &&
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.BankBranch>, IOrderedQueryable<Domain.Entities.BankBranch>> CreateOrderBy(GetListBankBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Bank,Country,City,District";
        }
    }
}