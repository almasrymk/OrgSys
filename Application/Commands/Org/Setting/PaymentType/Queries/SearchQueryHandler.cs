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

    public sealed record SearchPaymentTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<PaymentTypeModelView> ,ISearchQuery<ResultPagination<PaymentTypeModelView>>;

    public sealed class SearchQueryHandler(IRepository<Entity.Model.PaymentType> _Repository, IMapper mapper) : SearchCommandHandler<SearchPaymentTypeQuery, Entity.Model.PaymentType, PaymentTypeModelView>(_Repository, mapper)
    {
        public override Expression<Func<Entity.Model.PaymentType, bool>> CreateFilter(SearchPaymentTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Entity.Model.PaymentType>, IOrderedQueryable<Entity.Model.PaymentType>> CreateOrderBy(SearchPaymentTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}