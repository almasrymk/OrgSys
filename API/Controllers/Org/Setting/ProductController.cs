using Application.Commands.Org.Accounts.Account.Queries;
using Application.Commands.Org.Setting.Product.Commands;
using Application.Commands.Org.Setting.Product.Queries;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController(ISender sender) : BaseController<GetByIdProductQuery, SearchProductQuery , GetListProductQuery, CreateProductCommand, UpdateProductCommand, DeleteProductCommand, DeleteListProductCommand , GetMaxAccountQuery , ProductModelView>(sender)
    {
        [HttpGet("GetAllByBalance")]
        public virtual async Task<ResultCollection<ProductModelView>> GetAllByBalance(long StockId, DateTime date, CancellationToken cancellationToken)
        {
            return await sender.Send(new GetListProductByBalanceQuery(StockId, date), cancellationToken);
        }
    }
}