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
    using Utility;

    public sealed record GetListPaymentTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<PaymentTypeDto> , IListQuery<ResultCollection<PaymentTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<Domain.Entities.PaymentType> _Repository, IMapper mapper) : ListCommandHandler<GetListPaymentTypeQuery, Domain.Entities.PaymentType, PaymentTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<Domain.Entities.PaymentType, bool>> CreateFilter(GetListPaymentTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != Domain.Enums.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Domain.Entities.PaymentType>, IOrderedQueryable<Domain.Entities.PaymentType>> CreateOrderBy(GetListPaymentTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}