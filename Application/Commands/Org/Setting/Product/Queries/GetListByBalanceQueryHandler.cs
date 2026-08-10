namespace Application.Commands.Org.Setting.Product.Queries
{
    using Application.Abstraction.Command;
    using AutoMapper;
    using Domain.Abstraction;
    using Domain.Shared;
    using Application.DTOs;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed record GetListProductByBalanceQuery(long StockId, DateTime date) : ICommandCollection<ProductDto>;

    public sealed class GetListByBalanceQueryHandler(
        IRepository<Domain.Entities.TransactionProduct> _trnsRepository,
        IRepository<Domain.Entities.Product> productRepository,
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
