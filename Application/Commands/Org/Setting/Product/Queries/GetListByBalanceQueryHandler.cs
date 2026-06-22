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

    public sealed record GetListProductByBalanceQuery(long StockId, DateTime date) : ICommandCollection<ProductModelView>;

    public sealed class GetListByBalanceQueryHandler(IRepository<Domain.Entities.TransactionProduct> _trnsRepository, IMapper mapper) : ICommandCollectionHandler<GetListProductByBalanceQuery, ProductModelView>
    {        
        public async Task<ResultCollection<ProductModelView>> Handle(GetListProductByBalanceQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<ProductModelView> list = new List<ProductModelView>();
                var trns = await _trnsRepository.GetListByFilterAsync(e => e.StockId == request.StockId && e.Transaction.Date <= request.date , "Transaction,Transaction.Stock,Product,Unit,Product.ProductUnits");
                var products = trns!.Select(e => e.Product).Distinct().ToList();
                foreach (var product in products)
                {
                    var ob = mapper.Map<ProductModelView>(product);
                    ob.Balance = trns!.Where(e => e.ProductId == product.Id).Sum(e => e.Transaction.TypeId == 2 || e.Transaction.TypeId == 3 || e.Transaction.TypeId == 6 ? -1 * e.Quantity : e.Quantity);
                    list.Add(ob);
                }
               
                if (list != null)
                {
                    return new ResultCollection<ProductModelView>(
                    HttpStatusCode.OK,
                    list.ToList(),
                    null);
                }

                return new ResultCollection<ProductModelView>(
                    HttpStatusCode.InternalServerError,
                    new List<ProductModelView>(),
                    new List<Error> { new Error("Error") });
            }
            catch (Exception ex)
            {
                return new ResultCollection<ProductModelView>(
                    HttpStatusCode.InternalServerError,
                    new List<ProductModelView>(),
                    new List<Error> { new Error(ex.Message) });
            }
        }
    }
}