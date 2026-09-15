namespace Catalog.Application.PriceLists.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdPriceListQuery(long Id) : ICommand<PriceListDto>, IGetByIdQuery<Result<PriceListDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Catalog.Domain.PriceList> _Repository, IMapper mapper) : GetCommandHandler<GetByIdPriceListQuery, Catalog.Domain.PriceList, PriceListDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.PriceList, bool>> CreateFilter(GetByIdPriceListQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Entries,Entries.Product,Entries.Unit";
        }
    }
}
