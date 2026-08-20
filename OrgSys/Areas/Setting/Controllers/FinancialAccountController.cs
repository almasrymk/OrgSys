#nullable enable annotations
using Application.DTOs;
using Domain.Enums;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrgSys.Areas.Setting.Controllers;

[Area("Setting"), Authorize]
public sealed class FinancialAccountController(IConfiguration configuration, IHttpClientFactory httpClientFactory) : Controller
{
    private string ApiUrl => configuration["ApiUrl"] ?? string.Empty;

    public async Task<IActionResult> Index(string? search = null, FinancialAccountType? accountType = null)
    {
        var rows = await GetAccounts(accountType);
        if (!string.IsNullOrWhiteSpace(search))
            rows = rows.Where(e => (e.Code ?? "").Contains(search, StringComparison.OrdinalIgnoreCase)
                || e.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        ViewBag.Search = search;
        ViewBag.AccountType = accountType;
        return Request.Headers["X-Requested-With"] == "XMLHttpRequest"
            ? PartialView("List", rows) : View(rows);
    }

    public async Task<IActionResult> Save(long id = 0)
    {
        var model = id == 0 ? new FinancialAccountDto { IsActive = true, FinancialAccountType = FinancialAccountType.CashBox }
            : (await GetAccounts(null)).FirstOrDefault(e => e.Id == id) ?? new FinancialAccountDto();
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(FinancialAccountDto model)
    {
        if (!ModelState.IsValid) return View(model);
        using var client = httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"{ApiUrl}/Financial/Accounts", model);
        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index), new { status = ResultStatus.success, MsgError = "Success" });
        ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
        return View(model);
    }

    public async Task<JsonResult> GetList(string? txtSearch = null, FinancialAccountType? accountType = null)
    {
        var rows = await GetAccounts(accountType);
        if (!string.IsNullOrWhiteSpace(txtSearch))
            rows = rows.Where(e => e.Name.Contains(txtSearch, StringComparison.OrdinalIgnoreCase)
                || (e.Code ?? "").Contains(txtSearch, StringComparison.OrdinalIgnoreCase)).ToList();
        return Json(rows.Select(e => new { e.Id, e.Name, type = e.FinancialAccountType.ToString(), e.CurrencyId }));
    }

    private async Task<List<FinancialAccountDto>> GetAccounts(FinancialAccountType? accountType)
    {
        using var client = httpClientFactory.CreateClient();
        var url = $"{ApiUrl}/Financial/Accounts" + (accountType.HasValue ? $"?accountType={(int)accountType.Value}" : "");
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = JsonConvert.DeserializeObject<ResultCollection<FinancialAccountDto>>(await response.Content.ReadAsStringAsync());
        return result?.Response ?? [];
    }
}
