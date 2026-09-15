namespace Catalog.Application.Products.Queries
{
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Linq.Expressions;

    public sealed record GetByIdProductQuery(long Id) : ICommand<ProductDto> , IGetByIdQuery<Result<ProductDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Catalog.Domain.Product> _Repository, IMapper mapper) : GetCommandHandler<GetByIdProductQuery, Catalog.Domain.Product, ProductDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Product, bool>> CreateFilter(GetByIdProductQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Classification,Dealer,Brand,ProductUnits,ProductUnits.Unit,ProductPropertyElements";
        }
    }
}