using Application.Commands.Org.Setting.Preference.Queries;
using Application.Commands.Org.Setting.Stock.Commands;
using Application.Commands.Org.Setting.Stock.Queries;
using Application.Interfaces.CQRS;
using Azure;
using Domain.Shared;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Setting
{
    [ApiController]
    [Route("[controller]")]
    public class StockController(ISender sender) : BaseController<GetByIdStockQuery, SearchStockQuery , GetListStockQuery, CreateStockCommand, UpdateStockCommand, DeleteStockCommand, DeleteListStockCommand, StockModelView>(sender)
    {

    }
}