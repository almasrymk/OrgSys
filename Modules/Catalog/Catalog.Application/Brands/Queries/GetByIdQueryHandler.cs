namespace Catalog.Application.Brands.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdBrandQuery(long Id) : ICommand<BrandDto>, IGetByIdQuery<Result<BrandDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Catalog.Domain.Brand> _Repository, IMapper mapper) : GetCommandHandler<GetByIdBrandQuery, Catalog.Domain.Brand, BrandDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Brand, bool>> CreateFilter(GetByIdBrandQuery request)
        {
            return e => e.Id == request.Id && e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
    }
}
