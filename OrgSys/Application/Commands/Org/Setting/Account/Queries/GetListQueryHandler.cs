namespace Application.Commands.Org.Setting.Account.Queries
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

    public sealed record GetListAccountQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<AccountModelView> , IListQuery<ResultCollection<AccountModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Account> _Repository, IMapper mapper) : ListCommandHandler<GetListAccountQuery, Entity.Model.Account, AccountModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Account, bool>> CreateFilter(GetListAccountQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Account>, IOrderedQueryable<Entity.Model.Account>> CreateOrderBy(GetListAccountQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "AccountType";
        }
    }
}