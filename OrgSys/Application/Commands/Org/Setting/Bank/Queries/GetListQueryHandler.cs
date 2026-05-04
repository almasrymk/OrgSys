namespace Application.Commands.Org.Setting.Bank.Queries
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

    public sealed record GetListBankQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<BankModelView> , IListQuery<ResultCollection<BankModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.Bank> _Repository, IMapper mapper) : ListCommandHandler<GetListBankQuery, Entity.Model.Bank, BankModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.Bank, bool>> CreateFilter(GetListBankQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.Bank>, IOrderedQueryable<Entity.Model.Bank>> CreateOrderBy(GetListBankQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}