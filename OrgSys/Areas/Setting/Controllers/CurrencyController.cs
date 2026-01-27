namespace OrgSys.Areas.Setting.Controllers
{
    using Application.Commands.Org.Setting.Currency.Commands;
    using AutoMapper;
    using Entity.ModelView;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System.Threading.Tasks;

    [Area("Setting")]
    public class CurrencyController(IConfiguration configuration, IMapper mapper) : MainController<CurrencyModelView, CreateCurrencyCommand, UpdateCurrencyCommand>(configuration, mapper)
    {
        public async Task<JsonResult> GetRate(int id)
        {
            var ob = await GetObApi<CurrencyModelView>($"GetById?Id={id}");
            var data = new
            {
                rate = ob.Rate
            };
            return Json(data);
        }
    }
}