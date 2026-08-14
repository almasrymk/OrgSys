using Application.DTOs;
using Domain.Shared;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrgSys;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrgSys.Areas.Financial.Controllers;

[Area("Financials"), Authorize]
public sealed class FinancialTransferController(IConfiguration configuration) : Controller
{
    private string ApiUrl => configuration["ApiUrl"] ?? string.Empty;

    public async Task<IActionResult> Index(string search = "")
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"{ApiUrl}/FinancialTransfer/GetList");
        response.EnsureSuccessStatusCode();
        var result = JsonConvert.DeserializeObject<ResultCollection<FinancialTransferDto>>(await response.Content.ReadAsStringAsync());
        var rows = result?.Response ?? [];
        if (!string.IsNullOrWhiteSpace(search))
            rows = rows.Where(e => (e.FromFinancialAccountName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase)
                || (e.ToFinancialAccountName ?? "").Contains(search, StringComparison.OrdinalIgnoreCase)
                || (e.Description ?? "").Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        ViewBag.Search = search;
        return Request.Headers["X-Requested-With"] == "XMLHttpRequest"
            ? PartialView("List", rows) : View(rows);
    }

    public async Task<IActionResult> Save(long id = 0)
    {
        if (id == 0) return View(new FinancialTransferDto { TransactionDate = DateTime.Today, ExchangeRate = 1 });
        using var client = new HttpClient();
        var response = await client.GetAsync($"{ApiUrl}/FinancialTransfer/GetById?id={id}");
        response.EnsureSuccessStatusCode();
        var result = JsonConvert.DeserializeObject<Result<FinancialTransferDto>>(await response.Content.ReadAsStringAsync());
        return View(result?.Response ?? new FinancialTransferDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(FinancialTransferDto model)
    {
        if (model.Id > 0) return RedirectToAction(nameof(Index));
        model.CreateUserId = User.GetUserId();
        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync($"{ApiUrl}/FinancialTransfer/Post", model);
        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index), new { status = ResultStatus.success, MsgError = "Success" });
        ModelState.AddModelError(string.Empty, await response.Content.ReadAsStringAsync());
        return View(model);
    }
}
