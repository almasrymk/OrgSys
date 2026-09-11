namespace MasterData.Application.PaymentTypes.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record SearchPaymentTypeQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandPagination<PaymentTypeDto> ,ISearchQuery<ResultPagination<PaymentTypeDto>>;

    public sealed class SearchQueryHandler(IRepository<MasterData.Domain.PaymentType> _Repository, IMapper mapper) : SearchCommandHandler<SearchPaymentTypeQuery, MasterData.Domain.PaymentType, PaymentTypeDto>(_Repository, mapper)
    {
        public override Expression<Func<MasterData.Domain.PaymentType, bool>> CreateFilter(SearchPaymentTypeQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e => 
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<MasterData.Domain.PaymentType>, IOrderedQueryable<MasterData.Domain.PaymentType>> CreateOrderBy(SearchPaymentTypeQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }
    }
}
