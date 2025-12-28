namespace Application.Commands.Org.Setting.AccountType.Queries
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

    public sealed record GetListAccountTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<AccountTypeModelView> , IListQuery<ResultCollection<AccountTypeModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.AccountType> _Repository, IMapper mapper) : ListCommandHandler<GetListAccountTypeQuery, Entity.Model.AccountType, AccountTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.AccountType, bool>> CreateFilter(GetListAccountTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.AccountType>, IOrderedQueryable<Entity.Model.AccountType>> CreateOrderBy(GetListAccountTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}