using Application.Commands.Org.Financials.Financial.Commands;
using AutoMapper;
using Domain.Enums;
using Domain.Shared;
using Application.DTOs;
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
            1 => Domain.Resource.Title_Designer.OpeningBalance,
            2 => Domain.Resource.Title_Designer.Receipt,
            3 => Domain.Resource.Title_Designer.Payment,
            4 => Domain.Resource.Title_Designer.Transfer,
            5 => Domain.Resource.Title_Designer.Deposit,
            6 => Domain.Resource.Title_Designer.Withdrawal,
            7 => Domain.Resource.Title_Designer.Fee,
            8 => Domain.Resource.Title_Designer.Interest,
            9 => Domain.Resource.Title_Designer.Cheque,
            10 => Domain.Resource.Title_Designer.Adjustment,
            _ => Domain.Resource.Title_Designer.Financial
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
            ViewBag.FinancialAccountId = new SelectList(accountsResult?.Response ?? [], "Id", "Name", model.FinancialAccountId);

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

            ViewBag.CounterAccountId = new SelectList(await GetListApi<AccountDto>(), "Id", "Name", model.CounterAccountId);
            ViewBag.CurrencyId = new SelectList(await GetListApi<CurrencyDto>() , "Id", "Name", model.CurrencyId);

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
                ViewBag.FiscalYearId = new SelectList(fiscalYears, "Id", "Name", model.FiscalYearId);
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
            // Opening Balance doesn't create a Financial row at all — it writes a JournalItem line into the
            // shared per-fiscal-year Opening Balance Journal, the same mechanism Customer/Supplier opening
            // balances already use (see SetFinancialAccountOpeningBalanceCommandHandler).
            if (ob.FinancialTypeId == 1)
                return await SaveOpeningBalance(ob);

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
                    FinancialTypeId = ob.FinancialTypeId!.Value,
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
                    return RedirectToAction(nameof(Index), new { TypeId = command.FinancialTypeId, status = ResultStatus.success, MsgError = "Success" });
                ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
            }
            await LoadViewBag(ob);
            return View(ob);
        }

        private async Task<ActionResult> SaveOpeningBalance(FinancialDto ob)
        {
            if (ob.FinancialAccountId is null or <= 0)
                ModelState.AddModelError(nameof(ob.FinancialAccountId), "Financial account is required.");
            if (ob.FiscalYearId is null or <= 0)
                ModelState.AddModelError(nameof(ob.FiscalYearId), "Fiscal year is required.");
            if (ob.Debit < 0 || ob.Credit < 0)
                ModelState.AddModelError(string.Empty, "Debit and Credit must not be negative.");
            if (ob.Debit > 0 && ob.Credit > 0)
                ModelState.AddModelError(string.Empty, "Enter either Debit or Credit, not both.");
            if (ob.Debit == 0 && ob.Credit == 0)
                ModelState.AddModelError(string.Empty, "Enter a Debit or Credit amount greater than zero.");

            if (ModelState.IsValid)
            {
                var command = new
                {
                    FinancialAccountId = ob.FinancialAccountId!.Value,
                    FiscalYearId = ob.FiscalYearId!.Value,
                    Debit = ob.Debit,
                    Credit = ob.Credit,
                    CurrencyId = ob.CurrencyId,
                    Rate = ob.Rate,
                    Notes = ob.Notes,
                    CreateUserId = User.GetUserId()
                };
                using var client = CreateClient();
                var response = await client.PostAsJsonAsync($"{Configuration["ApiUrl"]}/Financial/FinancialAccount/OpeningBalance", command);
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Result<long>>(data);
                if (response.IsSuccessStatusCode && result is not null && (int)result.StatusCode is >= 200 and < 300)
                    // No dedicated Post/Reverse UI here — the shared Opening Balance journal is managed
                    // from the standard Journal screen, same as the existing Customer/Supplier flow.
                    return Redirect($"/Financials/Journal/Save?id={result.Response}");

                foreach (var error in result?.Errors ?? [])
                    ModelState.AddModelError(string.Empty, error.MessageError);
                if (result?.Errors is null or { Count: 0 })
                    ModelState.AddModelError(string.Empty, data);
            }
            await LoadViewBag(ob);
            return View("Save", ob);
        }

        public async Task<ActionResult> Cancel(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(ApiMethodType.Put, $"Cancel?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Domain.Shared.Result>(data);

            return Redirect("/Financials/Financial/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode == HttpStatusCode.OK ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }

        public async Task<ActionResult> Redo(long id, string search, long ParentId = 0, long TypeId = 0, int page = 1)
        {
            var response = await ApiMethod(ApiMethodType.Put, $"Redo?Id={id}");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<Domain.Shared.Result>(data);

            return Redirect("/Financials/Financial/Index?ParentId=" + ParentId + "&TypeId=" + TypeId + "&page=" + page + "&status=" + (res.StatusCode == HttpStatusCode.OK ? ResultStatus.success : ResultStatus.error) + "&MsgError=Success");
        }



        [HttpPost]
        public async Task<ActionResult> AutoSave(FinancialDto ob)
        {
            await base.Save(ob);
            if (ob.Id == 0)
            {

                var key = ob.GetType().GetProperty("Code")?.GetValue(ob, null);
                var searchResp = await ApiMethod(ApiMethodType.Get, $"Search?KeySearch={key}&ParentId={ob.ParentId}&TypeId={ob.TypeId}&Page=1&PageSize=1");
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
