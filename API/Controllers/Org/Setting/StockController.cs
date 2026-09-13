using Inventory.Application.Stocks.Commands;
using Inventory.Application.Stocks.Queries;
using OrgSys.SharedKernel;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class StockController(ISender sender) : BaseController<GetByIdStockQuery, SearchStockQuery , GetListStockQuery, CreateStockCommand, UpdateStockCommand, DeleteStockCommand, DeleteListStockCommand, StockDto>(sender)
    {

    }
}