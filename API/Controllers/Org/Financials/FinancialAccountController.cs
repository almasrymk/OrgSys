using Treasury.Application.FinancialAccounts.Commands;
using Treasury.Application.FinancialAccounts.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials
{
    // Standard CRUD shape (GetById/GetList/Search/Create/Update/Delete/DeleteList/GetMax) so the MVC side
    // can drive it through MainController<> exactly like Setting/Account, Setting/Currency, etc. — see
    // OrgSys/Areas/Setting/Controllers/FinancialAccountController.cs. The underlying handlers live under
    // Application/Commands/Org/Setting/FinancialAccount (same as Setting/Account) rather than Financials,
    // since this screen manages the account records themselves, not financial transactions. GetMax is
    // scoped by TypeId (Cash Box vs Bank get their own numbering), same convention Setting/Product and
    // Setting/Dealer use. The Balance lookup has no generic equivalent, so it stays as a one-off action
    // here (same pattern Financial/Redo and Financial/Cancel use).
    [Route("[controller]")]
    [ApiController]
    public class FinancialAccountController(ISender sender) : BaseController<
        GetByIdFinancialAccountQuery, SearchFinancialAccountQuery, GetListFinancialAccountQuery,
        CreateFinancialAccountCommand, UpdateFinancialAccountCommand,
        DeleteFinancialAccountCommand, DeleteListFinancialAccountCommand,
        GetMaxFinancialAccountQuery, FinancialAccountDto>(sender)
    {
        [HttpGet("{financialAccountId:long}/Balance")]
        public Task<Result<decimal>> Balance(long financialAccountId, DateTime? asOfDate, CancellationToken cancellationToken)
        {
            return Sender.Send(new GetFinancialAccountBalanceQuery(financialAccountId, asOfDate), cancellationToken);
        }
    }
}
