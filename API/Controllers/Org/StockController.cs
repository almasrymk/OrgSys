using Application.Commands.Org.Setting.Stock.Commands;
using Application.Commands.Org.Setting.Stock.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Stock
{
    [ApiController]
    [Route("[controller]")]
    public class StockController(ISender sender) : BaseController<GetByIdStockQuery, SearchStockQuery, CreateStockCommand, UpdateStockCommand, DeleteStockCommand, DeleteListStockCommand, StockModelView>(sender)
    {

    }
}
