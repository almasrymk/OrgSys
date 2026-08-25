#nullable enable annotations
using Application.DTOs;
using Domain.Enums;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using OrgSys;

namespace OrgSys.Areas.Setting.Controllers;

/// <summary>Cash Boxes and Bank Accounts screens — both are a <c>FinancialAccount</c> (CashBox/Bank
/// discriminated by <see cref="FinancialAccountType"/>) filtered by the generic <c>TypeId</c> filter
/// dimension (same convention <c>Financials/Financial</c> uses for its own sub-types), mirroring how
/// the Dealer controller multiplexes Client/Supplier under one entity. Persistence is delegated
/// entirely to the existing unified <c>SaveFinancialAccountCommand</c>/<c>SearchFinancialAccountsQuery</c>
/// engine (Application/Commands/Org/Financials/UnifiedFinancialCommands.cs) — no new data path. Index
/// follows the same contract as <see cref="OrgSys.Controllers.MainController{TDto,TCreate,TUpdate}.Index"/>
/// (same ViewBag names, same paged List partial, same shared search()/change() JS) so this screen behaves
/// identically to every other Setting master-data screen despite not inheriting that generic base — a
/// dual-entity write (FinancialAccount + Safe/BankAccount) doesn't fit the generic single-entity
/// Create/Update command shape.</summary>
[Area("Setting"), Authorize]
public sealed class FinancialAccountController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : Controller
{
    private string ApiUrl => configuration["ApiUrl"] ?? string.Empty;

    private static string PermissionPrefix(FinancialAccountType accountType) =>
        accountType == FinancialAccountType.Bank ? "BankAccounts" : "CashBoxes";

    // Mirrors MainController<>.OnActionExecuting so the shared search()/change() JS and the
    // "@ViewBag.area/@ViewBag.PageTitle/Save" link pattern used by every other Setting screen work here too.
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var areaName = "" + context.RouteData.Values["area"];
        ViewBag.Page = "/" + areaName + "/" + ControllerContext.ActionDescriptor.ControllerName;
        ViewBag.area = areaName;
        ViewBag.PageTitle = ControllerContext.ActionDescriptor.ControllerName;
        base.OnActionExecuting(context);
    }

    public async Task<IActionResult> Index(string search, long ParentId = 0, long TypeId = 0, int page = 1, int pageSize = 10, ResultStatus Status = ResultStatus.nothing, string MsgError = "")
    {
        var accountType = TypeId == (long)FinancialAccountType.Bank ? FinancialAccountType.Bank : FinancialAccountType.CashBox;
        if (!User.IsAllowed($"{PermissionPrefix(accountType)}.View"))
            return Forbid();

        if ("" + MsgError != "")
            ViewBag.message = MsgError;
        ViewBag.status = Status.ToString();
        ViewBag.pageNumber = page;
        ViewBag.ParentId = ParentId;
        ViewBag.TypeId = TypeId;
        ViewBag.AccountType = accountType;

        using var client = httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{ApiUrl}/Financial/Accounts/Search?KeySearch={search}&AccountType={(int)accountType}&Page={page}&PageSize={pageSize}");
        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadAsStringAsync();
        var dataList = JsonConvert.DeserializeObject<ResultPagination<FinancialAccountDto>>(data);

        return Request.Headers["X-Requested-With"] == "XMLHttpRequest"
            ? PartialView("List", dataList) : View("Index", dataList);
    }

    public async Task<IActionResult> Save(long id = 0, long TypeId = 0, ResultStatus status = ResultStatus.nothing, string MsgError = "")
    {
        var accountType = TypeId == (long)FinancialAccountType.Bank ? FinancialAccountType.Bank : FinancialAccountType.CashBox;
        var model = id == 0 ? new FinancialAccountDto { IsActive = true, FinancialAccountType = accountType }
            : (await GetAccounts(null, includeInactive: true)).FirstOrDefault(e => e.Id == id) ?? new FinancialAccountDto();

        if (!User.IsAllowed($"{PermissionPrefix(model.FinancialAccountType)}.{(id == 0 ? "Add" : "Edit")}"))
            return Forbid();

        ViewBag.TypeId = (long)model.FinancialAccountType;

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
            ViewBag.TypeId = (long)model.FinancialAccountType;
            await LoadViewBag();
            return View(model);
        }
        using var client = httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"{ApiUrl}/Financial/Accounts", model);
        if (response.IsSuccessStatusCode)
            return Redirect($"/{ViewBag.area}/{ViewBag.PageTitle}?TypeId={(long)model.FinancialAccountType}&Status={ResultStatus.success}&MsgError=Success");
        ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
        ViewBag.TypeId = (long)model.FinancialAccountType;
        await LoadViewBag();
        return View(model);
    }

    public async Task<Result> Delete(long id)
    {
        var account = (await GetAccounts(null, includeInactive: true)).FirstOrDefault(e => e.Id == id);
        if (account == null)
            return new Result(HttpStatusCode.NotFound, [new Error("Financial account not found.")]);
        if (!User.IsAllowed($"{PermissionPrefix(account.FinancialAccountType)}.Delete"))
            return new Result(HttpStatusCode.Forbidden, [new Error("Forbidden.")]);

        using var client = httpClientFactory.CreateClient();
        var response = await client.DeleteAsync($"{ApiUrl}/Financial/Accounts/Delete?Id={id}");
        if (response.IsSuccessStatusCode)
            return new Result(HttpStatusCode.OK, null);
        return new Result(HttpStatusCode.BadRequest, [new Error("Error")]);
    }

    [HttpPost]
    public async Task<Result> DeleteList(long[] ids, long TypeId = 0)
    {
        try
        {
            if (ids != null && ids.Length > 0)
            {
                var accountType = TypeId == (long)FinancialAccountType.Bank ? FinancialAccountType.Bank : FinancialAccountType.CashBox;
                if (!User.IsAllowed($"{PermissionPrefix(accountType)}.Delete"))
                    return new Result(HttpStatusCode.Forbidden, [new Error("Forbidden.")]);

                var query = string.Join("&", ids.Select(i => $"Ids={i}"));
                using var client = httpClientFactory.CreateClient();
                var response = await client.DeleteAsync($"{ApiUrl}/Financial/Accounts/DeleteList?{query}");
                if (response.IsSuccessStatusCode)
                    return new Result(HttpStatusCode.OK, null);
            }
        }
        catch (Exception ex)
        {
            return new Result(HttpStatusCode.InternalServerError, [new Error(ex.Message)]);
        }
        return new Result(HttpStatusCode.BadRequest, [new Error("Error")]);
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
