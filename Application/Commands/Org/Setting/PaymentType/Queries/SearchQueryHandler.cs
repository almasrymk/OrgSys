namespace Application.Commands.Org.Setting.PaymentType.Queries
{
    using Application.Abstraction.Command;
    using Application.Common.Queries;
    using Application.Interfaces.CQRS;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Linq.Expressions;

    public sealed record SearchPaymentTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<PaymentTypeDto> ,ISearchQuery<ResultPagination<PaymentTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<Domain.Entities.PaymentType> _Repository, IMapper mapper) : SearchCommandHandler<SearchPaymentTypeQuery, Domain.Entities.PaymentType, PaymentTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.PaymentType, bool>> CreateFilter(SearchPaymentTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.PaymentType>, IOrderedQueryable<Domain.Entities.PaymentType>> CreateOrderBy(SearchPaymentTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}