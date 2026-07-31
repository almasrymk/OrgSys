namespace Application.Commands.Org.Setting.Branch.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<BranchDto> ,ISearchQuery<ResultPagination<BranchDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.Branch> _Repository, IMapper mapper) : SearchCommandHandler<SearchBranchQuery, Domain.Entities.Branch, BranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Branch, bool>> CreateFilter(SearchBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name!.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Branch>, IOrderedQueryable<Domain.Entities.Branch>> CreateOrderBy(SearchBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}