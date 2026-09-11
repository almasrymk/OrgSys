using Inventory.Application.TransactionTypes.Queries;
using Inventory.Application.TransactionTypes.Commands;
using Inventory.Application.TransactionTypes.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Transaction
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionTypeController(ISender sender) :  BaseController<GetByIdTransactionTypeQuery , SearchTransactionTypeQuery, GetListTransactionTypeQuery,CreateTransactionTypeCommand, UpdateTransactionTypeCommand, DeleteTransactionTypeCommand, DeleteListTransactionTypeCommand, TransactionTypeDto>(sender)
    {
    }
}
