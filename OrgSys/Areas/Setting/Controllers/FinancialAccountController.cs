#nullable enable annotations
using Application.DTOs;
using Domain.Enums;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using OrgSys;

namespace OrgSys.Areas.Setting.Controllers;

/// <summary>Cash Boxes and Bank Accounts screens — both are a <c>FinancialAccount</c> (CashBox/Bank
/// discriminated by <see cref="FinancialAccountType"/>) filtered by <c>accountType</c>, mirroring how
/// the Dealer controller multiplexes Client/Supplier under one entity. Persistence is delegated
/// entirely to the existing unified <c>SaveFinancialAccountCommand</c>/<c>GetFinancialAccountsQuery</c>
/// engine (Application/Commands/Org/Financials/UnifiedFinancialCommands.cs) — no new data path.</summary>
[Area("Setting"), Authorize]
public sealed class FinancialAccountController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : Controller
{
    private string ApiUrl => configuration["ApiUrl"] ?? string.Empty;

    private static string PermissionPrefix(FinancialAccountType accountType) =>
        accountType == FinancialAccountType.Bank ? "BankAccounts" : "CashBoxes";

    public async Task<IActionResult> Index(string? search = null, FinancialAccountType? accountType = null)
    {
        if (!User.IsAllowed($"{PermissionPrefix(accountType ?? FinancialAccountType.CashBox)}.View"))
            return Forbid();

        var rows = await GetAccounts(accountType, includeInactive: true);
        if (!string.IsNullOrWhiteSpace(search))
            rows = rows.Where(e => (e.Code ?? "").Contains(search, StringComparison.OrdinalIgnoreCase)
                || e.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        ViewBag.Search = search;
        ViewBag.AccountType = accountType;
        return Request.Headers["X-Requested-With"] == "XMLHttpRequest"
            ? PartialView("List", rows) : View(rows);
    }

    public async Task<IActionResult> Save(long id = 0, FinancialAccountType accountType = FinancialAccountType.CashBox)
    {
        var model = id == 0 ? new FinancialAccountDto { IsActive = true, FinancialAccountType = accountType }
            : (await GetAccounts(null, includeInactive: true)).FirstOrDefault(e => e.Id == id) ?? new FinancialAccountDto();

        if (!User.IsAllowed($"{PermissionPrefix(model.FinancialAccountType)}.{(id == 0 ? "Add" : "Edit")}"))
            return Forbid();

        if (id > 0)
        {
            using var client = httpClientFactory.CreateClient();
            var balanceResponse = await client.GetAsync($"{ApiUrl}/Financial/Accounts/{id}/Balance");
            if (balanceResponse.IsSuccessStatusCode)
                ViewBag.Balance = JsonConvert.DeserializeObject<Result<decimal>>(
                    await balanceResponse.Content.ReadAsStringAsync())?.Response;
        }

        await LoadViewBag();
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(FinancialAccountDto model)
    {
        if (!User.IsAllowed($"{PermissionPrefix(model.FinancialAccountType)}.{(model.Id == 0 ? "Add" : "Edit")}"))
            return Forbid();

        if (!ModelState.IsValid)
        {
            await LoadViewBag();
            return View(model);
        }
        using var client = httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"{ApiUrl}/Financial/Accounts", model);
        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index), new { accountType = model.FinancialAccountType, status = ResultStatus.success, MsgError = "Success" });
        ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
        await LoadViewBag();
        return View(model);
    }

    public async Task<JsonResult> GetList(string? txtSearch = null, FinancialAccountType? accountType = null)
    {
        var rows = await GetAccounts(accountType, includeInactive: false);
        if (!string.IsNullOrWhiteSpace(txtSearch))
            rows = rows.Where(e => e.Name.Contains(txtSearch, StringComparison.OrdinalIgnoreCase)
                || (e.Code ?? "").Contains(txtSearch, StringComparison.OrdinalIgnoreCase)).ToList();
        return Json(rows.Select(e => new { e.Id, e.Name, type = e.FinancialAccountType.ToString(), e.CurrencyId }));
    }

    private async Task LoadViewBag()
    {
        ViewBag.AccountList = new SelectList(await GetListApi<AccountDto>("Account"), "Id", "Name");
        ViewBag.CurrencyList = new SelectList(await GetListApi<CurrencyDto>("Currency"), "Id", "Name");
        ViewBag.BranchList = new SelectList(await GetListApi<BranchDto>("Branch"), "Id", "Name");
        ViewBag.UserList = new SelectList(await GetListApi<UserDto>("User"), "Id", "Name");
        ViewBag.BankList = new SelectList(await GetListApi<BankDto>("Bank"), "Id", "Name");
        ViewBag.BankBranchList = await GetListApi<BankBranchDto>("BankBranch");
    }

    private async Task<List<TDto>> GetListApi<TDto>(string controllerName) where TDto : Domain.Entities.BaseModel
    {
        using var client = httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{ApiUrl}/{controllerName}/GetList?KeySearch=&ParentId=0&TypeId=0&Page=1&PageSize=500");
        if (!response.IsSuccessStatusCode) return [];
        var result = JsonConvert.DeserializeObject<ResultCollection<TDto>>(await response.Content.ReadAsStringAsync());
        return result?.Response ?? [];
    }

    private async Task<List<FinancialAccountDto>> GetAccounts(FinancialAccountType? accountType, bool includeInactive)
    {
        using var client = httpClientFactory.CreateClient();
        var url = $"{ApiUrl}/Financial/Accounts?includeInactive={includeInactive}" +
            (accountType.HasValue ? $"&accountType={(int)accountType.Value}" : "");
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = JsonConvert.DeserializeObject<ResultCollection<FinancialAccountDto>>(await response.Content.ReadAsStringAsync());
        return result?.Response ?? [];
    }
}
