using Application.Commands.Org.Transaction.Inventory.Queries;
using Application.Commands.Org.Transactions.Inventory.Commands;
using Application.Commands.Org.Transactions.Inventory.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Transaction
{
    [Route("[controller]")]
    [ApiController]
    public class InventoryController(ISender sender) : BaseController<GetByIdInventoryQuery ,SearchInventoryQuery, GetListInventoryQuery,CreateInventoryCommand,UpdateInventoryCommand,DeleteInventoryCommand, DeleteListInventoryCommand, GetMaxInventoryQuery,InventoryModelView>(sender)
    {

    }
}
