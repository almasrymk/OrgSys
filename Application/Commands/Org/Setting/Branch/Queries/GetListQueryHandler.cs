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
    using Utility;

    public sealed record GetListBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<BranchDto> , IListQuery<ResultCollection<BranchDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.Branch> _Repository, IMapper mapper) : ListCommandHandler<GetListBranchQuery, Domain.Entities.Branch, BranchDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.Branch, bool>> CreateFilter(GetListBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.Branch>, IOrderedQueryable<Domain.Entities.Branch>> CreateOrderBy(GetListBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}