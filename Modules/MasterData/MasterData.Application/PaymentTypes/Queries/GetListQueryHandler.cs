namespace MasterData.Application.PaymentTypes.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetListPaymentTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<PaymentTypeDto> , IListQuery<ResultCollection<PaymentTypeDto>>;

    public sealed class GetListQueryHandler(IRepository<MasterData.Domain.PaymentType> _Repository, IMapper mapper) : ListCommandHandler<GetListPaymentTypeQuery, MasterData.Domain.PaymentType, PaymentTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.PaymentType, bool>> CreateFilter(GetListPaymentTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<MasterData.Domain.PaymentType>, IOrderedQueryable<MasterData.Domain.PaymentType>> CreateOrderBy(GetListPaymentTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
