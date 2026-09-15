using Catalog.Application.Products.Commands;
using Catalog.Application.Products.Queries;
using Inventory.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(ISender sender) : BaseController<GetByIdProductQuery, SearchProductQuery , GetListProductQuery, CreateProductCommand, UpdateProductCommand, DeleteProductCommand, DeleteListProductCommand , GetMaxProductQuery , ProductDto>(sender)
    {
        [HttpGet("GetAllByBalance")]
        public virtual async Task<ResultCollection<ProductDto>> GetAllByBalance(long StockId, DateTime date, CancellationToken cancellationToken)
        {
            return await Sender.Send(new GetListProductByBalanceQuery(StockId, date), cancellationToken);
        }
    }
}