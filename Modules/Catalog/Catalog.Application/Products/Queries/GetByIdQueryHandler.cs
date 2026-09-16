namespace Catalog.Application.Products.Queries
{
    using Parties.Contracts.Dealers;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetByIdProductQuery(long Id) : ICommand<ProductDto> , IGetByIdQuery<Result<ProductDto>>;

    public sealed class GetByIdQueryHandler(IRepository<Catalog.Domain.Product> _Repository, IMapper mapper, ISender sender) : GetCommandHandler<GetByIdProductQuery, Catalog.Domain.Product, ProductDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Product, bool>> CreateFilter(GetByIdProductQuery request)
        {           
            return e => e.Id == request.Id && e.Status !=OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }

        public override string CreateInclude()
        {
            return "Classification,Brand,ProductUnits,ProductUnits.Unit,ProductPropertyElements";
        }

        public override async Task<Result<ProductDto>> Handle(GetByIdProductQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            if (result.Response is null || result.Response.DealerId is not > 0)
                return result;

            var names = (await sender.Send(new GetDealerNamesQuery([result.Response.DealerId.Value]), cancellationToken)).Response ?? [];
            result.Response.DealerName = names.GetValueOrDefault(result.Response.DealerId.Value);
            return result;
        }
    }
}
