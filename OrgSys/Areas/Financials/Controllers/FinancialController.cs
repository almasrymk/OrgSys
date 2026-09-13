using Treasury.Application.Financials.Commands;
using AutoMapper;
using OrgSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OrgSys.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Net.Http;

namespace OrgSys.Areas.Financial.Controllers
{
    [Area("Financials")]
    public class FinancialController(IConfiguration configuration, IMapper mapper) :
        MainController<FinancialDto, CreateFinancialCommand , UpdateFinancialCommand>(configuration , mapper)
    {
        // The FinancialType.Name seed values are raw English literals ("OpeningBalance", "Receipt", ...) —
        // not something to show a user. The menu (_MainMenu.cshtml) is the actual source of truth for what
        // each FinancialType is called, so this mirrors the same TypeId -> resource-key mapping used there
        // (and the same convention Invoice/FinancialAccount already use for their own dynamic titles).
        private static string ResolveFinancialTypeName(long typeId) => typeId switch
        {
            1 => OrgSys.Localization.Title_Designer.OpeningBalance,
            2 => OrgSys.Localization.Title_Designer.Receipt,
            3 => OrgSys.Localization.Title_Designer.Payment,
            4 => OrgSys.Localization.Title_Designer.TransferIn,
            5 => OrgSys.Localization.Title_Designer.Deposit,
            6 => OrgSys.Localization.Title_Designer.Withdrawal,
            7 => OrgSys.Localization.Title_Designer.Fee,
            8 => OrgSys.Localization.Title_Designer.Interest,
            9 => OrgSys.Localization.Title_Designer.Cheque,
            10 => OrgSys.Localization.Title_Designer.Adjustment,
            11 => OrgSys.Localization.Title_Designer.TransferOut,
            _ => OrgSys.Localization.Title_Designer.Financial
        };

        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            ViewBag.FinancialTypeName = ResolveFinancialTypeName(TypeId);
            // Icon isn't user-visible text (unlike Name), so the FinancialType table's own Icon column
            // is safe to use directly here — same approach Invoice takes with ViewBag.InvoicesIcon.
            var financialType = TypeId > 0 ? await GetObApi<FinancialTypeDto>($"GetById?Id={TypeId}") : null;
            ViewBag.FinancialTypeIcon = string.IsNullOrEmpty(financialType?.Icon) ? "iconsminds-coins" : financialType.Icon;
        }

        public override Task<FinancialDto> FixData(FinancialDto ob)
        {
            if (ob.Id == 0)
            {
                ob.CreateUserId = User.GetUserId();
                ob.CreateDate = DateTime.Now;
            }
            else
            {
                ob.ModifyUserId = User.GetUserId();
                ob.ModifyDate = DateTime.Now;
            }
            return base.FixData(ob);
        }
        public override async Task LoadViewBag(FinancialDto model)
        {

            using var client = CreateClient();
            var accountsResponse = await client.GetAsync($"{Configuration["ApiUrl"]}/Financial/Accounts");
            accountsResponse.EnsureSuccessStatusCode();
            var accountsResult = JsonConvert.DeserializeObject<ResultCollection<FinancialAccountDto>>(
                await accountsResponse.Content.ReadAsStringAsync());
            // Droptxt autocomplete needs the display name, not a SelectList — GetByIdFinancialQuery
            // deliberately doesn't include the FinancialAccount nav (see its CreateInclude comment), so
            // without this the field renders blank on edit even though FinancialAccountId is set.
            model.FinancialAccountName = accountsResult?.Response?.FirstOrDefault(a => a.Id == model.FinancialAccountId)?.Name;

            var currentFinancialTypeId = model.FinancialTypeId ?? model.TypeId;
            ViewBag.FinancialTypeName = ResolveFinancialTypeName(currentFinancialTypeId);

            // FinancialType.InOut is the real business rule for direction (1 = In-only, -1 = Out-only,
            // 0 = either) — see Infrastructure/Seed/InitialData.cs. Lock Direction to it on new records
            // instead of leaving it an open user choice; only types with InOut == 0 (Transfer/Cheque/
            // Adjustment) genuinely need the user to pick.
            var financialTypes = await GetListApi<FinancialTypeDto>();
            var currentFinancialType = financialTypes.FirstOrDefault(t => t.Id == currentFinancialTypeId);
            var inOut = currentFinancialType?.InOut ?? 0;
            ViewBag.FinancialTypeInOut = inOut;
            ViewBag.FinancialTypeIcon = string.IsNullOrEmpty(currentFinancialType?.Icon) ? "iconsminds-coins" : currentFinancialType.Icon;
            if (model.Id == 0 && inOut != 0)
                model.Direction = inOut > 0 ? FinancialTransactionDirection.In : FinancialTransactionDirection.Out;

            if (model.CounterAccountId > 0)
                model.CounterAccountName = (await GetObApi<AccountDto>($"GetById?Id={model.CounterAccountId}"))?.Name;
            ViewBag.CurrencyId = new SelectList(await GetListApi<CurrencyDto>() , "Id", "Name", model.CurrencyId);
            ViewBag.ReferenceTypeList = new SelectList(await GetListApi<ReferenceTypeDto>(), "Id", "Name", (long)model.ReferenceType);

            // Receipt/Payment only: resolve the currently-selected Reference's display name for the
            // Droptxt autocomplete on edit (mirrors FinancialAccountName/CounterAccountName above) —
            // the account itself is already carried on CounterAccountId/CounterAccountName, set by
            // FixReferenceAccount at Save time.
            if ((currentFinancialTypeId == (long)FinancialTransactionType.Receipt || currentFinancialTypeId == (long)FinancialTransactionType.Payment)
                && model.ReferenceId is > 0)
            {
                model.ReferenceName = await ResolveReferenceName(model.ReferenceType, model.ReferenceId.Value);
            }

            if (currentFinancialTypeId == 1)
            {
                var fiscalYears = await GetListApi<FiscalYearDto>();
                if (model.Id == 0 && model.FiscalYearId is null or <= 0)
                {
                    var defaultFiscalYear = fiscalYears.FirstOrDefault(f => f.IsCurrent)
                        ?? fiscalYears.FirstOrDefault(f => f.FiscalYearStatus == FiscalYearStatus.Open);
                    if (defaultFiscalYear is not null)
                    {
                        model.FiscalYearId = defaultFiscalYear.Id;
                        model.Date = defaultFiscalYear.StartDate;
                    }
                }
                else if (model.Id > 0 && model.FiscalYearId is null or <= 0)
                {
                    // FiscalYearId is never persisted on Financial (see FinancialDto's own comment) —
                    // on edit, re-derive it from Date for display only, the same way Post itself resolves
                    // the fiscal year. Without this the dropdown loads blank and re-saving an untouched
                    // Draft fails the "Fiscal year is required" check in SaveOpeningBalanceDraft.
                    var matchingFiscalYear = fiscalYears.FirstOrDefault(f =>
                        f.StartDate.Date <= model.Date.Date && f.EndDate.Date >= model.Date.Date);
                    if (matchingFiscalYear is not null)
                        model.FiscalYearId = matchingFiscalYear.Id;
                }
                ViewBag.FiscalYearId = new SelectList(fiscalYears, "Id", "Name", model.FiscalYearId);
                ViewBag.AllowPost = User.IsAllowed("Financial.Post");
                ViewBag.AllowReverse = User.IsAllowed("Financial.Reverse");

                // Resolved via its own lookup, not a loaded ModifyUser nav — GetByIdFinancialQuery
                // deliberately keeps Financial's own Include list minimal (see its CreateInclude comment).
                if (model.Posted && model.ModifyUserId is > 0)
                {
                    var postedByUser = await GetObApi<UserDto>($"GetById?Id={model.ModifyUserId}");
                    ViewBag.PostedByName = postedByUser?.Name;
                }
            }
        }


        public override async Task<FinancialDto> InitializeData(FinancialDto ob)
        {

            if (ob.Id == 0)
            {
                ob.Date = DateTime.Today;
                ob.Rate = 1;
                ob.Direction = FinancialTransactionDirection.In;
                ob.FinancialTypeId = ob.TypeId > 0 ? ob.TypeId : 1;
                ob.ReferenceType = FinancialReferenceType.Other;
            }

            return ob;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public override async Task<ActionResult> Save(FinancialDto ob)
        {
            // Opening Balance is a real Financial row like every other type, just Draft-first: Save here
            // only persists it (FixData + the generic Create/Update dispatch from MainController<>.Save,
            // same path AutoSave already uses) — no Journal, no Posted. Posting happens separately via the
            // Post action/PostFinancialOpeningBalanceCommand, mirroring Journal's own Draft/Post/Reverse flow.
            if (ob.FinancialTypeId == (long)Treasury.Domain.FinancialTransactionType.OpeningBalance)
                return await SaveOpeningBalanceDraft(ob);

            if (ob.FinancialAccountId is null or <= 0)
                ModelState.AddModelError(nameof(ob.FinancialAccountId), "Financial account is required.");
            if (ob.FinancialTypeId is null or <= 0)
                ModelState.AddModelError(nameof(ob.FinancialTypeId), "Financial type is required.");
            if (ob.Amount <= 0)
                ModelState.AddModelError(nameof(ob.Amount), "Amount must be greater than zero.");
            if (ob.CounterAccountId <= 0)
                ModelState.AddModelError(nameof(ob.CounterAccountId), "Counter GL account is required.");

            if (ModelState.IsValid)
            {
                var command = new PostFinancialTransactionDto
                {
                    FinancialAccountId = ob.FinancialAccountId!.Value,
                    FinancialTypeId = (Treasury.Domain.FinancialTransactionType)ob.FinancialTypeId!.Value,
                    Direction = ob.Direction ?? FinancialTransactionDirection.In,
                    Amount = ob.Amount,
                    CurrencyId = ob.CurrencyId,
                    ExchangeRate = ob.Rate,
                    TransactionDate = ob.Date,
                    ReferenceType = ob.ReferenceType,
                    ReferenceId = ob.ReferenceId,
                    CounterAccountId = ob.CounterAccountId,
                    Description = ob.Notes,
                    CreateUserId = User.GetUserId(),
                    BranchId = ob.BranchId,
                    ShiftId = ob.ShiftId
                };
                using var client = CreateClient();
                var response = await client.PostAsJsonAsync($"{Configuration["ApiUrl"]}/Financial/Transactions/Post", command);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index), new { TypeId = (long)command.FinancialTypeId, status = ResultStatus.success, MsgError = "Success" });
                ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
            }
            await LoadViewBag(ob);
            return View(ob);
        }

        private async Task<ActionResult> SaveOpeningBalanceDraft(FinancialDto ob)
        {
            if (ob.FinancialAccountId is null or <= 0)
                ModelState.AddModelError(nameof(ob.FinancialAccountId), "Financial account is required.");
            if (ob.FiscalYearId is null or <= 0)
                ModelState.AddModelError(nameof(ob.FiscalYearId), "Fiscal year is required.");
            if (ob.Amount <= 0)
                ModelState.AddModelError(nameof(ob.Amount), "Opening Balance amount must be greater than zero.");

            // Opening Balance has no free-form Direction/Reference/Counter-account/PaymentType choice —
            // locked exactly like the FinancialType.InOut-driven types (Deposit/Withdrawal/...), see
            // LoadViewBag above. PaymentTypeId is a required (non-nullable) FK on Financial with no field
            // on this screen to set it from — default to Cash (Id 1) or the insert fails with an FK violation.
            ob.Direction = FinancialTransactionDirection.In;
            ob.ReferenceType = FinancialReferenceType.Other;
            if (ob.PaymentTypeId <= 0)
                ob.PaymentTypeId = 1;
            ob.AmountByDefaultCurrency = ob.Amount * ob.Rate;

            return await base.Save(ob);
        }

        public async Task<ActionResult> Post(long id, long ParentId = 0, long TypeId = 0)
        {
            if (!User.IsAllowed("Financial.Post"))
                return Forbid();

            var response = await ApiMethod(HttpMethod.Put, $"Post?Id={id}&UserId={User.GetUserId()}");
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<OrgSys.SharedKernel.Result>(data);

            if (res?.StatusCode == HttpStatusCode.OK)
                return Redirect($"/Financials/Financial/Index?ParentId={ParentId}&TypeId={TypeId}&status={ResultStatus.success}&MsgError=Success");

            var message = string.Join(" ", res?.Errors?.Select(e => e.MessageError) ?? []);
            return Redirect($"/Financials/Financial/Save?id={id}&ParentId={ParentId}&TypeId={TypeId}&status={ResultStatus.error}&MsgError={Uri.EscapeDataString(message)}");
        }

        public async Task<ActionResult> Reverse(long id, long ParentId = 0, long TypeId = 0)
        {
            if (!User.IsAllowed("Financial.Reverse"))
                return Forbid();

            var response = await ApiMethod(HttpMethod.Put, $"Reverse?Id={id}");
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<OrgSys.SharedKernel.Result>(data);

            if (res?.StatusCode == HttpStatusCode.OK)
                return Redirect($"/Financials/Financial/Index?ParentId={ParentId}&TypeId={TypeId}&status={ResultStatus.success}&MsgError=Success");

            var message = string.Join(" ", res?.Errors?.Select(e => e.MessageError) ?? []);
            return Redirect($"/Financials/Financial/Save?id={id}&ParentId={ParentId}&TypeId={TypeId}&status={ResultStatus.error}&MsgError={Uri.EscapeDataString(message)}");
        }

        public async Task<ActionResult> Cancel(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(HttpMethod.Put, $"Cancel?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<OrgSys.SharedKernel.Result>(data);

            return Redirect("/Financials/Financial/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode == HttpStatusCode.OK ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }

        public async Task<ActionResult> Redo(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(HttpMethod.Put, $"Redo?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<OrgSys.SharedKernel.Result>(data);

            return Redirect("/Financials/Financial/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode == HttpStatusCode.OK ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }

        // Receipt/Payment's Reference picker (Droptxt autocomplete): the source table is chosen by
        // referenceType, reusing the same GetListApi<> calls the rest of the app already uses for these
        // entities — no new table, no new AppService. accountId/accountName (when the reference has one)
        // let the client auto-fill the readonly Account field without a second round-trip; the server
        // still re-resolves and validates this authoritatively at Post time (PostTransactionCommandHandler),
        // this is UI convenience only. Employee/Loan/Cheque/PaymentGateway have no backing entity in this
        // codebase yet, so they fall through to an empty list — same as an unrecognized referenceType.
        public async Task<JsonResult> GetReferenceList(int referenceType, string txtSearch = "")
        {
            txtSearch = (txtSearch ?? "").Trim();
            var list = new List<object>();

            switch ((FinancialReferenceType)referenceType)
            {
                case FinancialReferenceType.Customer:
                case FinancialReferenceType.Supplier:
                {
                    var dealerTypeId = (FinancialReferenceType)referenceType == FinancialReferenceType.Customer ? 1 : 2;
                    var dealers = await GetListApi<DealerDto>(TypeId: dealerTypeId, TextSearch: txtSearch, PageSize: 20);
                    list = dealers.Select(d => (object)new
                    {
                        id = d.Id,
                        name = $"{d.Code} - {d.Name}",
                        accountId = d.AccountId,
                        accountName = d.AccountId is > 0 ? $"{d.AccountCode} - {d.AccountName}" : null
                    }).ToList();
                    break;
                }

                case FinancialReferenceType.Expense:
                case FinancialReferenceType.Income:
                {
                    var expectedTypeName = (FinancialReferenceType)referenceType == FinancialReferenceType.Expense ? "Expense" : "Revenue";
                    var accounts = await GetListApi<AccountDto>(TextSearch: txtSearch, PageSize: 200);
                    list = accounts
                        .Where(a => a.IsPostable && string.Equals(a.AccountTypeName, expectedTypeName, StringComparison.OrdinalIgnoreCase))
                        .Take(20)
                        .Select(a => (object)new { id = a.Id, name = $"{a.Code} - {a.Name}", accountId = (long?)a.Id, accountName = $"{a.Code} - {a.Name}" })
                        .ToList();
                    break;
                }

                case FinancialReferenceType.Invoice:
                {
                    var invoices = await GetListApi<InvoiceDto>(TextSearch: txtSearch, PageSize: 20);
                    var dealerNames = (await GetListApi<DealerDto>(PageSize: 5000)).ToDictionary(d => d.Id, d => d.Name);
                    list = invoices.Select(inv => (object)new
                    {
                        id = inv.Id,
                        name = $"{inv.Code} - {(dealerNames.TryGetValue(inv.DealerId, out var dn) ? dn : "")} - {inv.Credit:N2}",
                        accountId = (long?)null,
                        accountName = (string?)null
                    }).ToList();
                    break;
                }

                case FinancialReferenceType.Payment:
                {
                    var payments = await GetListApi<FinancialDto>(TypeId: (long)FinancialTransactionType.Payment, TextSearch: txtSearch, PageSize: 20);
                    list = payments.Select(f => (object)new { id = f.Id, name = $"{f.Code} - {f.Amount:N2}", accountId = (long?)null, accountName = (string?)null }).ToList();
                    break;
                }

                case FinancialReferenceType.Transfer:
                {
                    var transfersIn = await GetListApi<FinancialDto>(TypeId: (long)FinancialTransactionType.TransferIn, TextSearch: txtSearch, PageSize: 20);
                    var transfersOut = await GetListApi<FinancialDto>(TypeId: (long)FinancialTransactionType.TransferOut, TextSearch: txtSearch, PageSize: 20);
                    list = transfersIn.Concat(transfersOut)
                        .Select(f => (object)new { id = f.Id, name = $"{f.Code} - {f.Amount:N2}", accountId = (long?)null, accountName = (string?)null })
                        .ToList();
                    break;
                }
            }

            return Json(list);
        }

        // Resolves the display label for a Financial's currently-set ReferenceId on edit — mirrors
        // GetReferenceList's per-type source, just fetching the single already-selected record.
        private async Task<string?> ResolveReferenceName(FinancialReferenceType type, long referenceId)
        {
            switch (type)
            {
                case FinancialReferenceType.Customer:
                case FinancialReferenceType.Supplier:
                {
                    var dealer = await GetObApi<DealerDto>($"GetById?Id={referenceId}");
                    return dealer is null ? null : $"{dealer.Code} - {dealer.Name}";
                }

                case FinancialReferenceType.Expense:
                case FinancialReferenceType.Income:
                {
                    var account = await GetObApi<AccountDto>($"GetById?Id={referenceId}");
                    return account is null ? null : $"{account.Code} - {account.Name}";
                }

                case FinancialReferenceType.Invoice:
                {
                    var invoice = await GetObApi<InvoiceDto>($"GetById?Id={referenceId}");
                    if (invoice is null)
                        return null;
                    var dealerName = invoice.DealerId > 0 ? (await GetObApi<DealerDto>($"GetById?Id={invoice.DealerId}"))?.Name : null;
                    return $"{invoice.Code} - {dealerName} - {invoice.Credit:N2}";
                }

                case FinancialReferenceType.Payment:
                case FinancialReferenceType.Transfer:
                {
                    var financial = await GetObApi<FinancialDto>($"GetById?Id={referenceId}");
                    return financial is null ? null : $"{financial.Code} - {financial.Amount:N2}";
                }

                default:
                    return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult> AutoSave(FinancialDto ob)
        {
            await base.Save(ob);
            if (ob.Id == 0)
            {

                var key = ob.GetType().GetProperty("Code")?.GetValue(ob, null);
                var searchResp = await ApiMethod(HttpMethod.Get, $"Search?KeySearch={key}&ParentId={ob.ParentId}&TypeId={ob.TypeId}&Page=1&PageSize=1");
                if (searchResp != null && searchResp.IsSuccessStatusCode)
                {
                    var searchData = await searchResp.Content.ReadAsStringAsync();
                    var searchRes = JsonConvert.DeserializeObject<ResultPagination<FinancialDto>>(searchData);
                    if (searchRes != null && searchRes.Response != null && searchRes.Response.Count > 0)
                    {
                        ob.Id = searchRes.Response[0].Id;
                        ob.CreateUserId = searchRes.Response[0].CreateUserId;
                    }

                    return Ok(new
                    {
                        status = "success",
                        id = ob.Id,
                        createdUserId = ob.CreateUserId,
                        url = "/" + "Financial" + "/" + "Financial" + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success"
                    });
                }
            }
            return Ok();
        }
    }
}
