using Application.DTOs;
using Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrgSys.ViewComponents;

public sealed class MainMenuViewComponent(IConfiguration configuration, IHttpClientFactory httpClientFactory) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var financialTypes = new List<FinancialTypeDto>();
        try
        {
            using var client = httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{configuration["ApiUrl"]}/FinancialType/GetList?ParentId=0&TypeId=0&Page=1&PageSize=100");
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<ResultCollection<FinancialTypeDto>>(
                    await response.Content.ReadAsStringAsync());
                financialTypes = result?.Response?.Where(e => !e.Hide).OrderBy(e => e.Id).ToList() ?? [];
            }
        }
        catch
        {
            // Keep the rest of the menu available if the API is temporarily unavailable.
        }

        return View("~/Views/Shared/_MainMenu.cshtml", financialTypes);
    }
}
