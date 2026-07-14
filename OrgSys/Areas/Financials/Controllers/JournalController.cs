namespace OrgSys.Areas.Financial.Controllers
{
    using Application.Commands.Org.Financials.Journal.Commands;
    using Application.DTOs;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using OrgSys.Controllers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Financials")]
    public class JournalController(IConfiguration configuration, IMapper mapper) : MainController<JournalDto, CreateJournalCommand, UpdateJournalCommand>(configuration, mapper)
    {
        public override async Task LoadViewBag(JournalDto model)
        {
            ViewBag.CurrencyId = new SelectList(await GetListApi<CurrencyDto>(), "Id", "Name", model.CurrencyId);
        }

        public override Task<JournalDto> InitializeData(JournalDto model)
        {
            if (model.Id == 0)
            {
                model.Date = DateTime.Now;
                model.JournalItems = new List<JournalItemDto>();
            }
            else
            {
                model.JournalItems ??= new List<JournalItemDto>();
            }

            return Task.FromResult(model);
        }

        public async Task<JsonResult> GetList(string txtSearch = "", int page = 1, int pageSize = 20)
        {
            if (txtSearch != null)
                txtSearch = txtSearch.Trim().ToLower();

            var itemsList = await GetListApi<JournalDto>(TextSearch: txtSearch, Page: page, PageSize: pageSize);
            var list = itemsList.Distinct().OrderBy(_ => _.Code)
                .Select(_ => new
                {
                    _.Id,
                    _.Code
                })
                .ToList();
            return Json(list);
        }
    }
}
