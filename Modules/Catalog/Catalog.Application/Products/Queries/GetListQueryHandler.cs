namespace Catalog.Application.Products.Queries
{
    using Parties.Contracts.Dealers;
    using OrgSys.SharedKernel;
    using AutoMapper;
    using MediatR;
    using System.Linq.Expressions;

    public sealed record GetListProductQuery(string KeySearch, long ParentId, long TypeId, int Page , int PageSize) : ICommandCollection<ProductDto> , IListQuery<ResultCollection<ProductDto>>;

    public sealed class GetListQueryHandler(IRepository<Catalog.Domain.Product> _Repository, IMapper mapper, ISender sender) : ListCommandHandler<GetListProductQuery, Catalog.Domain.Product, ProductDto>(_Repository, mapper)
    {
        public override Expression<Func<Catalog.Domain.Product, bool>> CreateFilter(GetListProductQuery request)
        {
            Page = request.Page;
            PageSize = request.PageSize;

            return e =>
            (string.IsNullOrEmpty(request.KeySearch) || e.Name.Contains(request.KeySearch)) &&
            e.Status != OrgSys.SharedKernel.Status.Deleted && e.Hide != true;
        }
         
        override public Func<IQueryable<Catalog.Domain.Product>, IOrderedQueryable<Catalog.Domain.Product>> CreateOrderBy(GetListProductQuery request)
        {
            return q => q.OrderByDescending(e => e.Id);
        }

        public override string CreateInclude()
        {
            return "Classification,Brand,ProductUnits,ProductPropertyElements";
        }

        public override async Task<ResultCollection<ProductDto>> Handle(GetListProductQuery request, CancellationToken cancellationToken)
        {
            var result = await base.Handle(request, cancellationToken);
            var dealerIds = result.Response.Where(e => e.DealerId is > 0).Select(e => e.DealerId!.Value).Distinct().ToList();
            if (dealerIds.Count > 0)
            {
                var names = (await sender.Send(new GetDealerNamesQuery(dealerIds), cancellationToken)).Response ?? [];
                foreach (var dto in result.Response)
                    if (dto.DealerId is > 0)
                        dto.DealerName = names.GetValueOrDefault(dto.DealerId.Value);
            }

            return result;
        }
    }
}
