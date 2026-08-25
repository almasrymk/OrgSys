using Application.Commands.Org.Financials.Financial.Commands;
using Application.Commands.Org.Financials.Financial.Queries;
using Application.Commands.Org.Financials.Receivable.Commands;
using Application.Commands.Org.Financials.Unified;
using Domain.Shared;
using Application.DTOs;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials
{
    [Route("[controller]")]
    [ApiController]
    public class FinancialController(ISender sender) : BaseController<GetByIdFinancialQuery, SearchFinancialQuery, GetListFinancialQuery,
        CreateFinancialCommand, UpdateFinancialCommand, DeleteFinancialCommand, DeleteListFinancialCommand,
        GetMaxFinancialQuery, FinancialDto>(sender)
    {

        [HttpPut("Redo")]
        public async Task<Result> Redo(long Id, CancellationToken cancellationToken)
        {
            return await Sender.Send(new RedoFinancialCommand(Id), cancellationToken);
        }

        [HttpPut("Cancel")]
        public async Task<Result> Cancel(long Id, CancellationToken cancellationToken)
        {
            return await Sender.Send(new CancelFinancialCommand(Id), cancellationToken);
        }

        [HttpGet("Accounts")]
        public Task<ResultCollection<FinancialAccountDto>> Accounts(
            FinancialAccountType? accountType,
            bool includeInactive,
            CancellationToken cancellationToken)
        {
            return Sender.Send(new GetFinancialAccountsQuery(accountType, includeInactive), cancellationToken);
        }

        [HttpGet("Accounts/{financialAccountId:long}/Balance")]
        public Task<Result<decimal>> Balance(
            long financialAccountId,
            DateTime? asOfDate,
            CancellationToken cancellationToken)
        {
            return Sender.Send(new GetFinancialAccountBalanceQuery(financialAccountId, asOfDate), cancellationToken);
        }

        [HttpPost("Accounts")]
        public Task<Result> SaveAccount(
            [FromBody] FinancialAccountDto account,
            CancellationToken cancellationToken)
        {
            return Sender.Send(new SaveFinancialAccountCommand(account), cancellationToken);
        }

        [HttpPost("Transactions/Post")]
        public Task<Result> PostTransaction(
            [FromBody] PostFinancialTransactionDto transaction,
            CancellationToken cancellationToken)
        {
            return Sender.Send(new PostFinancialTransactionCommand(transaction), cancellationToken);
        }

        [HttpPost("Receivable/OpeningBalance")]
        public Task<Result> SetCustomerOpeningBalance(
            [FromBody] SetCustomerOpeningBalanceCommand command,
            CancellationToken cancellationToken)
        {
            return Sender.Send(command, cancellationToken);
        }
    }
}
