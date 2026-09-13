namespace OrgSys.Areas.Setting.Controllers
{
    using MasterData.Application.Currencies.Commands;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class CurrencyController(IConfiguration configuration, IMapper mapper) : MainController<CurrencyDto, CreateCurrencyCommand, UpdateCurrencyCommand>(configuration, mapper)
    {
        public async Task<JsonResult> GetRate(int id)
        {
            var ob = await GetObApi<CurrencyDto>($"GetById?Id={id}");
            var data = new
            {
                rate = ob.Rate
            };
            return Json(data);
        }
    }
}