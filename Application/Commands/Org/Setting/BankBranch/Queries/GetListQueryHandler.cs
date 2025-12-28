namespace Application.Commands.Org.Setting.BankBranch.Queries
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

    public sealed record GetListBankBranchQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<BankBranchModelView> , IListQuery<ResultCollection<BankBranchModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.BankBranch> _Repository, IMapper mapper) : ListCommandHandler<GetListBankBranchQuery, Entity.Model.BankBranch, BankBranchModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.BankBranch, bool>> CreateFilter(GetListBankBranchQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.BankBranch>, IOrderedQueryable<Entity.Model.BankBranch>> CreateOrderBy(GetListBankBranchQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}