namespace Inventory.Application.Products.Queries
{
    using Catalog.Contracts.Products;
    using MediatR;
    using OrgSys.SharedKernel;
    using System.Net;

    public sealed record GetListProductByBalanceQuery(long StockId, DateTime date) : ICommandCollection<ProductBalanceDto>;

    public sealed class GetListByBalanceQueryHandler(
        IRepository<Inventory.Domain.TransactionProduct> _trnsRepository,
        ISender sender) : ICommandCollectionHandler<GetListProductByBalanceQuery, ProductBalanceDto>
    {
        public async Task<ResultCollection<ProductBalanceDto>> Handle(GetListProductByBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var catalog = (await sender.Send(new GetProductCatalogQuery(), cancellationToken)).Response ?? [];
                var trns = (await _trnsRepository.GetListByFilterAsync(
                    e => e.StockId == request.StockId && e.Transaction!.Date <= request.date,
                    "Transaction"))?.ToList() ?? [];

                var list = catalog.Select(product => new ProductBalanceDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Nickname = product.Nickname,
                    Barcode = product.Barcode,
                    Price = product.Price,
                    Cost = product.Cost,
                    ClassificationId = product.ClassificationId,
                    ClassificationName = product.ClassificationName,
                    ProductUnits = product.Units,
                    Balance = trns.Where(e => e.ProductId == product.Id)
                        .Sum(e => e.Transaction!.TypeId == 2 || e.Transaction.TypeId == 3 || e.Transaction.TypeId == 6 || e.Transaction.TypeId == 8
                            ? -e.Quantity
                            : e.Quantity)
                }).ToList();

                return new ResultCollection<ProductBalanceDto>(HttpStatusCode.OK, list, null);
            }
            catch (Exception ex)
            {
                return new ResultCollection<ProductBalanceDto>(
                    HttpStatusCode.InternalServerError,
                    new List<ProductBalanceDto>(),
                    new List<Error> { new Error(ex.Message) });
            }
        }
    }
}
