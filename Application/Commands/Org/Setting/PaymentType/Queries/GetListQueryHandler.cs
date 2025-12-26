namespace Application.Commands.Org.Setting.PaymentType.Queries
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

    public sealed record GetListPaymentTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<PaymentTypeModelView> , IListQuery<ResultCollection<PaymentTypeModelView>>;

    public sealed class GetListQueryHandler(IRepository<Entity.Model.PaymentType> _Repository, IMapper mapper) : ListCommandHandler<GetListPaymentTypeQuery, Entity.Model.PaymentType, PaymentTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.PaymentType, bool>> CreateFilter(GetListPaymentTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.PaymentType>, IOrderedQueryable<Entity.Model.PaymentType>> CreateOrderBy(GetListPaymentTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}