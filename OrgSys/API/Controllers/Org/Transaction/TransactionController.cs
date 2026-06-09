using Application.Commands.Org.Setting.Transaction.Queries;
using Application.Commands.Org.Transactions.Transaction.Commands;
using Application.Commands.Org.Transactions.Transaction.Queries;
using Application.Commands.Org.Transactions.TransactionType.Commands;
using Entity.ModelView;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Transaction
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController(ISender sender) : BaseController<GetByIdTransactionQuery, SearchTransactionQuery ,GetListTransactionQuery,CreateTransactionCommand,UpdateTransactionCommand ,DeleteTransactionCommand, DeleteListTransactionCommand ,GetMaxTransactionQuery, TransactionModelView>(sender)
    {


    }
}
