namespace Inventory.Application.Products.Queries
{
    using OrgSys.SharedKernel;
    using AutoMapper;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Catalog.Application;

    /// <summary>
    /// Stays in Inventory.Application (not Catalog) because it reads Inventory-owned
    /// TransactionProduct/Transaction data to compute a per-product stock balance — a genuinely
    /// Inventory-side computation, even though Product itself is now Catalog-owned. Reaches
    /// Catalog.Domain.Product/ProductDto for the product side, same accepted-exception shape
    /// Inventory already has for other relocated master data (see
    /// docs/catalog/catalog-target-architecture.md §4).
    /// </summary>
    public sealed record GetListProductByBalanceQuery(long StockId, DateTime date) : ICommandCollection<ProductDto>;

    public sealed class GetListByBalanceQueryHandler(
        IRepository<Inventory.Domain.TransactionProduct> _trnsRepository,
        IRepository<Catalog.Domain.Product> productRepository,
        IMapper mapper) : ICommandCollectionHandler<GetListProductByBalanceQuery, ProductDto>
    {
        public async Task<ResultCollection<ProductDto>> Handle(GetListProductByBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<ProductDto> list = new List<ProductDto>();
                var trns = (await _trnsRepository.GetListByFilterAsync(
                    e => e.StockId == request.StockId && e.Transaction!.Date <= request.date,
                    "Transaction,Product,Unit"))?.ToList() ?? [];
                var products = (await productRepository.GetListByFilterAsync(
                    e => true,
                    "ProductUnits,ProductUnits.Unit"))?.ToList() ?? [];

                foreach (var product in products)
                {
                    var ob = mapper.Map<ProductDto>(product);
                    ob.Balance = trns.Where(e => e.ProductId == product.Id)
                        .Sum(e => e.Transaction!.TypeId == 2 || e.Transaction.TypeId == 3 || e.Transaction.TypeId == 6 || e.Transaction.TypeId == 8
                            ? -e.Quantity
                            : e.Quantity);
                    list.Add(ob);
                }

                if (list != null)
                {
                    return new ResultCollection<ProductDto>(
                    HttpStatusCode.OK,
                    list.ToList(),
                    null);
                }

                return new ResultCollection<ProductDto>(
                    HttpStatusCode.InternalServerError,
                    new List<ProductDto>(),
                    new List<Error> { new Error("Error") });
            }
            catch (Exception ex)
            {
                return new ResultCollection<ProductDto>(
                    HttpStatusCode.InternalServerError,
                    new List<ProductDto>(),
                    new List<Error> { new Error(ex.Message) });
            }
        }
    }
}
