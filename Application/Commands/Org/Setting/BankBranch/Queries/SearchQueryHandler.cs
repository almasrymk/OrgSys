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

    public sealed record SearchBankBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<BankBranchDto> ,ISearchQuery<ResultPagination<BankBranchDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.BankBranch> _Repository, IMapper mapper) : SearchCommandHandler<SearchBankBranchQuery, Domain.Entities.BankBranch, BankBranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.BankBranch, bool>> CreateFilter(SearchBankBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.BankBranch>, IOrderedQueryable<Domain.Entities.BankBranch>> CreateOrderBy(SearchBankBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Bank,Country,City,District";
        }
    }
}