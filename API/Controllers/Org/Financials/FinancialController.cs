using Application.Commands.Org.Financials.Financial.Commands;
using Application.Commands.Org.Financials.Financial.Queries;
using Application.Commands.Org.Financials.Receivable.Commands;
using Application.Commands.Org.Setting.FinancialAccount.Commands;
using Application.Commands.Org.Setting.FinancialAccount.Queries;
using AutoMapper;
using Domain.Shared;
using Application.DTOs;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials
{
    [Route("[controller]")]
    [ApiController]
    public class FinancialController(ISender sender, IMapper mapper) : BaseController<GetByIdFinancialQuery, SearchFinancialQuery, GetListFinancialQuery,
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
        public async Task<ResultCollection<FinancialAccountDto>> Accounts(
            FinancialAccountType? accountType,
            bool includeInactive,
            CancellationToken cancellationToken)
        {
            var typeId = accountType.HasValue ? (long)accountType.Value : 0;
            var res = await Sender.Send(new GetListFinancialAccountQuery("", 0, typeId, 1, int.MaxValue), cancellationToken);
            if (!includeInactive && res.Response != null)
                res = res with { Response = res.Response.Where(e => e.IsActive).ToList() };
            return res;
        }

        [HttpGet("Accounts/Search")]
        public Task<ResultPagination<FinancialAccountDto>> SearchAccounts(
            string? KeySearch,
            FinancialAccountType? AccountType,
            int Page,
            int PageSize,
            CancellationToken cancellationToken)
        {
            var typeId = AccountType.HasValue ? (long)AccountType.Value : 0;
            return Sender.Send(new SearchFinancialAccountQuery(KeySearch ?? "", 0, typeId, Page, PageSize), cancellationToken);
        }

        [HttpDelete("Accounts/Delete")]
        public Task<Result> DeleteAccount(long Id, CancellationToken cancellationToken)
        {
            return Sender.Send(new DeleteFinancialAccountCommand(Id), cancellationToken);
        }

        [HttpDelete("Accounts/DeleteList")]
        public Task<Result> DeleteAccountList([FromQuery] List<long> Ids, CancellationToken cancellationToken)
        {
            return Sender.Send(new DeleteListFinancialAccountCommand(Ids), cancellationToken);
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
            return account.Id == 0
                ? Sender.Send(mapper.Map<CreateFinancialAccountCommand>(account), cancellationToken)
                : Sender.Send(mapper.Map<UpdateFinancialAccountCommand>(account), cancellationToken);
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
