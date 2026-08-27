using Application.Commands.Org.Financials.FinancialAccount.Commands;
using Application.Commands.Org.Financials.FinancialAccount.Queries;
using Application.Commands.Org.Financials.Unified;
using Application.DTOs;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Org.Financials
{
    // Standard CRUD shape (GetById/GetList/Search/Create/Update/Delete/DeleteList/GetMax) so the MVC side
    // can drive it through MainController<> exactly like Setting/Account, Setting/Currency, etc. — see
    // OrgSys/Areas/Setting/Controllers/FinancialAccountController.cs. GetMax is scoped by TypeId (Cash Box
    // vs Bank get their own numbering), same convention Setting/Product and Setting/Dealer use. The Balance
    // lookup has no generic equivalent, so it stays as a one-off action here (same pattern Financial/Redo
    // and Financial/Cancel use).
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
