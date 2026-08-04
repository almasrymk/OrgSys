namespace OrgSys.Areas.Financial.Controllers
{
    using Application.Commands.Org.Financials.Journal.Commands;
    using Application.DTOs;
    using AutoMapper;
    using Domain.Enums;
    using Domain.Shared;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using OrgSys.Controllers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Area("Financials")]
    public class JournalController(IConfiguration configuration, IMapper mapper) : MainController<JournalDto, CreateJournalCommand, UpdateJournalCommand>(configuration, mapper)
    {
        public override async Task LoadViewBagIndex(long ParentId = 0, long TypeId = 0)
        {
            ViewBag.InvoicesTypes = await GetListApi<InvoiceTypeDto>(); ;
            ViewBag.TransactionsTypes = await GetListApi<TransactionTypeDto>(); ;           
        }

        public override async Task LoadViewBag(JournalDto model)
        {
            ViewBag.JournalTypeId = new SelectList(await GetListApi<JournalTypeDto>(), "Id", "Name", model.JournalTypeId);
            ViewBag.CurrencyId = new SelectList(await GetListApi<CurrencyDto>(), "Id", "Name", model.CurrencyId);
        }

        public override async Task<JournalDto> InitializeData(JournalDto ob)
        {
            var preferenceList = await GetListApi<PreferenceDto>(TypeId: ob.TypeId, TextSearch: "Journal", PageSize: 1000);
            var DefaultCurrencyId = long.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultCurrency")?.Value);
            var DefaultJournalTypeId = long.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "DefaultJournalType")?.Value);
            ViewBag.NumberLine = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "NumberLine")?.Value);
            ViewBag.OrderTabe = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "OrderTabe")?.Value);
            ViewBag.AutoSave = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "AutoSave")?.Value);
            var TypeCode = int.Parse("0" + preferenceList.FirstOrDefault(e => e.Key == "TypeSerial")?.Value);
            ViewBag.TypeSerial = TypeCode;

            if (ob == null)
                ob = new JournalDto();

            if (ob.Id == 0)
            {
                ob.CodeNumber = long.Parse("0" + await GetValueApi<JournalDto>($"GetMax?ParentId=0&TypeId={ob.TypeId}")) + 1;
                ob.Code = "" + ob.CodeNumber;               
                ob.CurrencyId = DefaultCurrencyId;
                ob.JournalTypeId = DefaultJournalTypeId;
                ob.Date = DateTime.Now;
                ob.Rate = (await GetObApi<CurrencyDto>($"GetById?Id={ob.CurrencyId}"))?.Rate ?? 0;
                ob.JournalItems = new List<JournalItemDto>();
            }

            if (ob.JournalItems == null)
                ob.JournalItems = new List<JournalItemDto>();
             
            return ob;
        }

        public override Task<JournalDto> FixData(JournalDto ob)
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

        [HttpPost]
        public async Task<ActionResult> AutoSave(JournalDto ob)
        {
            await base.Save(ob);
            if (ob.Id == 0)
            {

                var key = ob.GetType().GetProperty("Code")?.GetValue(ob, null);
                var searchResp = await ApiMethod(ApiMethodType.Get, $"Search?KeySearch={key}&ParentId={ob.ParentId}&TypeId={ob.TypeId}&Page=1&PageSize=1");
                if (searchResp != null && searchResp.IsSuccessStatusCode)
                {
                    var searchData = await searchResp.Content.ReadAsStringAsync();
                    var searchRes = JsonConvert.DeserializeObject<ResultPagination<InvoiceDto>>(searchData);
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
                        url = "/" + "Financials" + "/" + "Journal" + "?ParentId=" + ob.ParentId + "&TypeId=" + ob.TypeId + "&status=" + ResultStatus.success + "&MsgError=Success"
                    });
                }
            }
            return Ok();
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
