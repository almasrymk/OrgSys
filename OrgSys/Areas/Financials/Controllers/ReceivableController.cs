using Application.DTOs;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OrgSys.Areas.Financials.Controllers
{
    /// <summary>Customer Receipt / Collection screen — a thin proxy over the API's
    /// Financial/Receivable/Receipt endpoints (Application/Commands/Org/Financials/Receivable).
    /// Not a plain CRUD entity, so this deliberately does not inherit MainController&lt;TDto,...&gt;.</summary>
    [Area("Financials")]
    [Authorize]
    public class ReceivableController(IConfiguration configuration) : Controller
    {
        private HttpClient CreateClient() =>
            HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>().CreateClient();

        private string ApiUrl => configuration["ApiUrl"]!;

        public async Task<IActionResult> Index(long dealerId = 0)
        {
            ViewBag.Dealers = new SelectList(await GetClientsAsync(), "Id", "Name", dealerId);
            ViewBag.DealerId = dealerId;

            if (dealerId <= 0)
                return View(new List<FinancialDto>());

            using var client = CreateClient();

            var balanceResponse = await client.GetAsync($"{ApiUrl}/Dealer/Balance?Id={dealerId}");
            if (balanceResponse.IsSuccessStatusCode)
                ViewBag.Balance = JsonConvert.DeserializeObject<Result<decimal>>(
                    await balanceResponse.Content.ReadAsStringAsync())?.Response;

            var receiptsResponse = await client.GetAsync($"{ApiUrl}/Financial/Receivable/Receipts?dealerId={dealerId}");
            receiptsResponse.EnsureSuccessStatusCode();
            var receipts = JsonConvert.DeserializeObject<ResultCollection<FinancialDto>>(
                await receiptsResponse.Content.ReadAsStringAsync());

            return View(receipts?.Response ?? []);
        }

        public async Task<IActionResult> Save(long dealerId)
        {
            ViewBag.Dealers = new SelectList(await GetClientsAsync(), "Id", "Name", dealerId);
            ViewBag.FinancialAccounts = new SelectList(await GetFinancialAccountsAsync(), "Id", "Name");

            return View(new PostCustomerReceiptDto { DealerId = dealerId, Date = DateTime.Today, ExchangeRate = 1, CurrencyId = 1 });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(PostCustomerReceiptDto model)
        {
            model.CreateUserId = User.GetUserId();

            using (var client = CreateClient())
            {
                var response = await client.PostAsJsonAsync($"{ApiUrl}/Financial/Receivable/Receipt", model);
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Result>(data);

                if (result?.StatusCode == HttpStatusCode.OK)
                    return RedirectToAction("Index", new { dealerId = model.DealerId });

                ModelState.AddModelError(string.Empty,
                    string.Join(" ", result?.Errors?.Select(e => e.MessageError) ?? ["An error occurred while posting the receipt."]));
            }

            ViewBag.Dealers = new SelectList(await GetClientsAsync(), "Id", "Name", model.DealerId);
            ViewBag.FinancialAccounts = new SelectList(await GetFinancialAccountsAsync(), "Id", "Name", model.FinancialAccountId);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Reverse(long id, long dealerId)
        {
            using var client = CreateClient();
            await client.PutAsync($"{ApiUrl}/Financial/Receivable/Receipt/Reverse?Id={id}", null);
            return RedirectToAction("Index", new { dealerId });
        }

        private async Task<List<DealerDto>> GetClientsAsync()
        {
            using var client = CreateClient();
            var response = await client.GetAsync($"{ApiUrl}/Dealer/GetList?KeySearch=&ParentId=0&TypeId=1&Page=1&PageSize=500");
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<ResultCollection<DealerDto>>(await response.Content.ReadAsStringAsync());
            return result?.Response ?? [];
        }

        private async Task<List<FinancialAccountDto>> GetFinancialAccountsAsync()
        {
            using var client = CreateClient();
            var response = await client.GetAsync($"{ApiUrl}/Financial/Accounts");
            response.EnsureSuccessStatusCode();
            var result = JsonConvert.DeserializeObject<ResultCollection<FinancialAccountDto>>(await response.Content.ReadAsStringAsync());
            return result?.Response ?? [];
        }
    }
}
